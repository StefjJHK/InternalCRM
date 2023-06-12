using BIP.InternalCRM.Domain.Subscriptions;
using BIP.InternalCRM.Persistence.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BIP.InternalCRM.Persistence.Domain.Invoices;

[DbConfiguration(typeof(DomainDbContext))]
public class SubscriptionDbConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> sub)
    {
        sub.HasKey(_ => _.Id);
        sub.Property(_ => _.Id)
            .ValueGeneratedNever();

        sub.HasIndex(_ => _.Number)
            .IsUnique();

        sub.Ignore(_ => _.DomainEvents);

        sub.OwnsOne(
            _ => _.License,
            lic =>
            {
                lic.Ignore(_ => _.Data)
                    .WithOwner();
            });
    }
}
