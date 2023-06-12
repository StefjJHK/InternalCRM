using BIP.InternalCRM.Domain.Products;

namespace BIP.InternalCRM.Shopify.Entities;

public record ShopifyProductsRelations(
    ShopifyProductId ShopifyProductId,
    ProductId DomainProductId
);
