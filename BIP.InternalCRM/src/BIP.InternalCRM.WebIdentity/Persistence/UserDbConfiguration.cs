using BIP.InternalCRM.Persistence.Attributes;
using BIP.InternalCRM.WebIdentity.Roles;
using BIP.InternalCRM.WebIdentity.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BIP.InternalCRM.WebIdentity.Persistence;

[DbConfiguration(typeof(AuthDbContext))]
public class UserDbConfiguration :
    IEntityTypeConfiguration<User>,
    IEntityTypeConfiguration<IdentityUserClaim<Guid>>,
    IEntityTypeConfiguration<IdentityUserRole<Guid>>,
    IEntityTypeConfiguration<IdentityUserLogin<Guid>>,
    IEntityTypeConfiguration<IdentityUserToken<Guid>>

{
    public void Configure(EntityTypeBuilder<User> user)
    {
        user.ToTable("Users");

        user.HasKey(_ => _.Id);

        user.HasMany(_ => _.Roles)
            .WithMany(_ => _.Users)
            .UsingEntity<IdentityUserRole<Guid>>();
    }

    public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> userRole)
    {
        userRole.ToTable("UserRoles");

        userRole.HasKey(_ => new { _.RoleId, _.UserId });

        userRole.HasOne<Role>()
            .WithMany()
            .HasForeignKey(_ => _.RoleId)
            .OnDelete(DeleteBehavior.NoAction);

        userRole.HasOne<User>()
            .WithMany()
            .HasForeignKey(_ => _.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }

    public void Configure(EntityTypeBuilder<IdentityUserClaim<Guid>> userClaim)
    {
        userClaim.ToTable("UserClaims");

        userClaim.HasKey(_ => _.Id);

        userClaim.HasOne<User>()
            .WithMany()
            .HasForeignKey(_ => _.UserId);
    }

    public void Configure(EntityTypeBuilder<IdentityUserLogin<Guid>> userLogin)
    {
        userLogin.ToTable("UserLogins");

        userLogin.HasKey(_ => _.UserId);

        userLogin.HasOne<User>()
            .WithOne()
            .HasForeignKey<IdentityUserLogin<Guid>>(_ => _.UserId);
    }

    public void Configure(EntityTypeBuilder<IdentityUserToken<Guid>> userToken)
    {
        userToken.ToTable("UserTokens");

        userToken.HasKey(_ => _.UserId);

        userToken.HasOne<User>()
            .WithOne()
            .HasForeignKey<IdentityUserToken<Guid>>(_ => _.UserId);
    }
}