using BIP.InternalCRM.Persistence.Attributes;
using BIP.InternalCRM.Shopify;
using BIP.InternalCRM.Shopify.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BIP.InternalCRM.Persistence.Shopify;

[DbConfiguration(typeof(IShopifyDbContext))]
public class ShopifyProductsRelationsDbConfiguration :
    IEntityTypeConfiguration<ShopifyProductsRelations>
{
    public void Configure(EntityTypeBuilder<ShopifyProductsRelations> productsRelations)
    {
        productsRelations.ToTable("ProductsRelations", "shopify");
        
        productsRelations.HasKey(_ => _.ShopifyProductId);
        productsRelations.Property(_ => _.ShopifyProductId)
            .HasConversion(
                id => id.Value,
                value => new ShopifyProductId(value));
        
        productsRelations.HasIndex(_ => _.DomainProductId)
            .IsUnique();
    }
}