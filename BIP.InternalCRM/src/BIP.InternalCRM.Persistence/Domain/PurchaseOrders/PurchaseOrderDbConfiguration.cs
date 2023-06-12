using BIP.InternalCRM.Domain.Customers;
using BIP.InternalCRM.Domain.Products;
using BIP.InternalCRM.Domain.PurchaseOrders;
using BIP.InternalCRM.Persistence.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BIP.InternalCRM.Persistence.Domain.PurchaseOrders;

[DbConfiguration(typeof(DomainDbContext))]
public class PurchaseOrderDbConfiguration : IEntityTypeConfiguration<PurchaseOrder>
{
    public void Configure(EntityTypeBuilder<PurchaseOrder> po)
    {
        po.HasKey(_ => _.Id);
        po.Property(_ => _.Id)
            .ValueGeneratedNever();

        po.HasIndex(_ => _.Number)
            .IsUnique();

        po.Ignore(_ => _.DomainEvents);

        po.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(_ => _.CustomerId);

        po.HasOne<Product>()
            .WithMany()
            .HasForeignKey(_ => _.ProductId);

        po.HasMany(_ => _.Invoices)
            .WithOne(_ => _.PurchaseOrder);
    }
}
