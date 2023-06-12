using AutoMapper;
using BIP.InternalCRM.Shopify.AccessService.Dto;
using BIP.InternalCRM.Shopify.Entities;
using BIP.InternalCRM.Shopify.Http;

namespace BIP.InternalCRM.Shopify.AccessService;

public interface IShopifyAccessService
{
    Task<ShopifyProductAccessDto> GetProductAsync(ShopifyProductId productId, CancellationToken cancellationToken = default);

    Task<ICollection<ShopifyProductAccessDto>> GetProductsAsync(CancellationToken cancellationToken = default);
}
public class ShopifyAccessService : IShopifyAccessService
{
    private readonly IShopifyHttpClient _httpClient;

    public ShopifyAccessService(IShopifyHttpClient httpClient, IMapper mapper)
    {
        _httpClient = httpClient;
    }

    public async Task<ShopifyProductAccessDto> GetProductAsync(ShopifyProductId productId, CancellationToken cancellationToken = default)
    {
        var result = await _httpClient.GetAsync<ShopifyProductAccessDto>(
            $"products/{productId.Value}.json",
            "$.product",
            cancellationToken);

        return result;
    }

    public async Task<ICollection<ShopifyProductAccessDto>> GetProductsAsync(CancellationToken cancellationToken = default)
    {
        var result = await _httpClient.GetAsync<ShopifyProductAccessDto[]>(
            $"products.json",
            "$.products",
            cancellationToken);

        return result;
    }
}
