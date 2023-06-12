using BIP.InternalCRM.Persistence.Attributes;
using BIP.InternalCRM.WebIdentity.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BIP.InternalCRM.WebIdentity.Persistence;

[DbConfiguration(typeof(AuthDbContext))]
public class RoleDbConfiguration :
    IEntityTypeConfiguration<Role>,
    IEntityTypeConfiguration<IdentityRoleClaim<Guid>>
{
    public void Configure(EntityTypeBuilder<Role> role)
    {
        role.ToTable("Roles");

        role.HasKey(_ => _.Id);

        role.HasMany(_ => _.Users)
            .WithMany(_ => _.Roles)
            .UsingEntity<IdentityUserRole<Guid>>();
    }

    public void Configure(EntityTypeBuilder<IdentityRoleClaim<Guid>> roleClaim)
    {
        roleClaim.ToTable("RoleClaims");

        roleClaim.HasKey(_ => _.Id);
        roleClaim.HasOne<Role>()
            .WithMany()
            .HasForeignKey(_ => _.RoleId);
    }
}