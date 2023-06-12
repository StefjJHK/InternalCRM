using BIP.InternalCRM.Domain.Customers;
using BIP.InternalCRM.Domain.Invoices;
using BIP.InternalCRM.Domain.Products;
using BIP.InternalCRM.Persistence.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BIP.InternalCRM.Persistence.Domain.Invoices;

[DbConfiguration(typeof(DomainDbContext))]
public class InvoiceDbConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> invoice)
    {
        invoice.HasKey(_ => _.Id);
        invoice.Property(_ => _.Id)
            .ValueGeneratedNever();

        invoice.HasIndex(_ => _.Number)
            .IsUnique();
        
        invoice.Ignore(_ => _.DomainEvents);

        invoice.HasOne(_ => _.PurchaseOrder)
            .WithMany(_ => _.Invoices);

        invoice.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(_ => _.CustomerId);

        invoice.HasOne<Product>()
            .WithMany()
            .HasForeignKey(_ => _.ProductId);

        invoice.HasMany(_ => _.Payments)
            .WithOne()
            .HasForeignKey(_ => _.InvoiceId);

        invoice.HasMany(_ => _.Subscriptions)
            .WithOne()
            .HasForeignKey(_ => _.InvoiceId);
    }
}
