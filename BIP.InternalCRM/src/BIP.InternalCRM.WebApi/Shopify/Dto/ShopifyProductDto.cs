namespace BIP.InternalCRM.WebApi.Shopify.Dto;

public record ShopifyProductDto(
    ulong Id,
    string Name,
    string Status
);
