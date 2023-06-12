using BIP.InternalCRM.Application.Customers;
using BIP.InternalCRM.Domain.Customers;
using BIP.InternalCRM.Domain.Products;
using BIP.InternalCRM.Persistence.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BIP.InternalCRM.Persistence.Domain.Clientele;

[DbConfiguration(typeof(DomainDbContext))]
public class CustomerDbConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> customer)
    {
        customer.HasKey(_ => _.Id);
        customer.Property(_ => _.Id)
            .ValueGeneratedNever();

        customer.HasIndex(_ => _.Name)
            .IsUnique();

        customer.Ignore(_ => _.DomainEvents);

        customer.OwnsOne(_ => _.ContactInfo)
            .WithOwner();
        customer.Navigation(_ => _.ContactInfo)
            .IsRequired();

        customer.HasMany<Product>()
            .WithMany()
            .UsingEntity<CustomerRelations>();
    }
}