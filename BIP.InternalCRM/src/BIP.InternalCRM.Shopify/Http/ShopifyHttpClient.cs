using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BIP.InternalCRM.Shopify.Http;

#nullable enable

public interface IShopifyHttpClient
{
    Task<TOutput> GetAsync<TOutput>(string uri, string? jsonPath = null, CancellationToken cancellationToken = default);
}

public class ShopifyHttpClient : IShopifyHttpClient
{
    private readonly ShopifyOptions _options;
    private readonly HttpClient _httpClient;

    public ShopifyHttpClient(
        IHttpClientFactory httpCleintFactory,
        IOptions<ShopifyOptions> optionsAccessor)
    {
        _options = optionsAccessor.Value;

        _httpClient = httpCleintFactory.CreateClient(nameof(ShopifyHttpClient));
        _httpClient.BaseAddress = new Uri(_options.ShopBaseUrl, $"/admin/api/{_options.ApiVersion}/");
        _httpClient.DefaultRequestHeaders.Add("X-Shopify-Access-Token", _options.AccessToken);
    }

    public async Task<TOutput> GetAsync<TOutput>(string uri, string? jsonPath = null, CancellationToken cancellationToken = default)
    {
        var response = await SendAsync(HttpMethod.Get, uri, null, cancellationToken);

        response.EnsureSuccessStatusCode();

        return await ReadAsJsonAsync<TOutput>(response.Content, jsonPath);
    }

    private async Task<HttpResponseMessage> SendAsync(
        HttpMethod method,
        string uri,
        HttpContent? body,
        CancellationToken cancellationToken = default)
    {

        var requestMessage = new HttpRequestMessage(method, uri)
        { Content = body };

        return await _httpClient.SendAsync(requestMessage, cancellationToken);
    }

    private static async Task<T> ReadAsJsonAsync<T>(HttpContent httpContent, string? jsonPath = null)
    {
        var content = await httpContent.ReadAsStringAsync();

        if (jsonPath == null)
        {
            return JsonConvert.DeserializeObject<T>(content)!;
        }

        var json = JsonConvert.DeserializeObject<JObject>(content);
        return json!.SelectToken(jsonPath)!.ToObject<T>()!;
    }
}
