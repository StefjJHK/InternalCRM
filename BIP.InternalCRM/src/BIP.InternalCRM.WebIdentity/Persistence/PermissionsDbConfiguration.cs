using BIP.InternalCRM.Persistence.Attributes;
using BIP.InternalCRM.WebIdentity.Permissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BIP.InternalCRM.WebIdentity.Persistence;

[DbConfiguration(typeof(AuthDbContext))]
public class PermissionsDbConfiguration : IEntityTypeConfiguration<PermissionSet>
{
    private const string KeyName = "UserRoleId";

    public void Configure(EntityTypeBuilder<PermissionSet> perms)
    {
        perms.ToTable("Permissions");

        perms.Property<Guid>(KeyName);
        perms.HasKey(KeyName);
        
        perms.OwnsOne(_ => _.Analytics)
            .WithOwner();
        perms.Navigation(_ => _.Analytics)
            .IsRequired();

        perms.OwnsOne(_ => _.Product)
            .WithOwner();
        perms.Navigation(_ => _.Product)
            .IsRequired();

        perms.OwnsOne(_ => _.Customer)
            .WithOwner();
        perms.Navigation(_ => _.Customer)
            .IsRequired();
        
        perms.OwnsOne(_ => _.Lead)
            .WithOwner();
        perms.Navigation(_ => _.Lead)
            .IsRequired();
                
        perms.OwnsOne(_ => _.PurchaseOrder)
            .WithOwner();
        perms.Navigation(_ => _.PurchaseOrder)
            .IsRequired();
        
        perms.OwnsOne(_ => _.Invoice)
            .WithOwner();
        perms.Navigation(_ => _.Invoice)
            .IsRequired();
        
        perms.OwnsOne(_ => _.Payment)
            .WithOwner();
        perms.Navigation(_ => _.Payment)
            .IsRequired();
        
        perms.OwnsOne(_ => _.Subscription)
            .WithOwner();
        perms.Navigation(_ => _.Subscription)
            .IsRequired();
    }
}
