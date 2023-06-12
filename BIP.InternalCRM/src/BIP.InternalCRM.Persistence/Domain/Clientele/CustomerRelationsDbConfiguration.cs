using BIP.InternalCRM.Application.Customers;
using BIP.InternalCRM.Persistence.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BIP.InternalCRM.Persistence.Domain.Clientele;


[DbConfiguration(typeof(DomainDbContext))]
public class CustomerRelationsDbConfiguration :
    IEntityTypeConfiguration<CustomerRelations>
{
    public void Configure(EntityTypeBuilder<CustomerRelations> customerRelations)
    {
        customerRelations.HasKey(_ => new { _.CustomerId, _.ProductId });
    }
}