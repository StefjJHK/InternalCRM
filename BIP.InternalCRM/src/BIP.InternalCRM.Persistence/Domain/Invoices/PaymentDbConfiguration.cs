using BIP.InternalCRM.Domain.Payments;
using BIP.InternalCRM.Persistence.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BIP.InternalCRM.Persistence.Domain.Invoices;

[DbConfiguration(typeof(DomainDbContext))]
public class PaymentDbConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> payment)
    {
        payment.HasKey(_ => _.Id);
        payment.Property(_ => _.Id)
            .ValueGeneratedNever();

        payment.HasIndex(_ => _.Number)
            .IsUnique();

        payment.Ignore(_ => _.DomainEvents);
    }
}
