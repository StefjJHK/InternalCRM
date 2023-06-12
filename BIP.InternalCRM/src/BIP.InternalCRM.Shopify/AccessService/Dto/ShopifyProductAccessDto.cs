using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BIP.InternalCRM.Shopify.AccessService.Dto;

public record ShopifyProductAccessDto
{
    [JsonProperty("id")]
    public ulong ShopifyId { get; init; }

    [JsonProperty("title")]
    public string Name { get; init; }

    [JsonProperty("status")]
    public string Status { get; init; }

    [JsonProperty("options")]
    public JArray Options { get; init; }
}
