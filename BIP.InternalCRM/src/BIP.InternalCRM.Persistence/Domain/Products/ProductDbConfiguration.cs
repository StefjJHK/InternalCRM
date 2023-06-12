using BIP.InternalCRM.Application.Customers;
using BIP.InternalCRM.Application.Statistics.Products.Models;
using BIP.InternalCRM.Domain.Customers;
using BIP.InternalCRM.Domain.Products;
using BIP.InternalCRM.Persistence.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BIP.InternalCRM.Persistence.Domain.Products;

[DbConfiguration(typeof(DomainDbContext))]
public class ProductDbConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> product)
    {
        product.HasKey(_ => _.Id);
        product.Property(_ => _.Id)
            .ValueGeneratedNever();

        product.HasIndex(_ => _.Name)
            .IsUnique();

        product.Ignore(_ => _.DomainEvents);

        product.OwnsOne(
            _ => _.Icon,
            icon =>
            {
                icon.Ignore(_ => _.Data)
                    .WithOwner();
            });
        
        product.OwnsOne(
            _ => _.Project,
            proj =>
            {
                proj.Ignore(_ => _.Data)
                    .WithOwner();
            });

        product.HasMany<Customer>()
            .WithMany()
            .UsingEntity<CustomerRelations>();
    }
}
