using BIP.InternalCRM.Domain.Leads;
using BIP.InternalCRM.Domain.Products;
using BIP.InternalCRM.Persistence.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BIP.InternalCRM.Persistence.Domain.Clientele;

[DbConfiguration(typeof(DomainDbContext))]
public class LeadDbConfiguration : IEntityTypeConfiguration<Lead>
{
    public void Configure(EntityTypeBuilder<Lead> lead)
    {
        lead.HasKey(_ => _.Id);
        lead.Property(_ => _.Id)
            .ValueGeneratedNever();

        lead.HasIndex(_ => _.Name)
            .IsUnique();

        lead.Ignore(_ => _.DomainEvents);

        lead.HasOne<Product>()
            .WithMany()
            .HasForeignKey(_ => _.ProductId);

        lead.OwnsOne(_ => _.ContactInfo)
            .WithOwner();
        lead.Navigation(_ => _.ContactInfo)
            .IsRequired();
    }
}
