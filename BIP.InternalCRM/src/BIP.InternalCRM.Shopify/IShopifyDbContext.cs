using BIP.InternalCRM.Shopify.Entities;
using Microsoft.EntityFrameworkCore;

namespace BIP.InternalCRM.Shopify;

public interface IShopifyDbContext
{
    DbSet<ShopifyProductsRelations> ProductsRelations { get; init; }
}
