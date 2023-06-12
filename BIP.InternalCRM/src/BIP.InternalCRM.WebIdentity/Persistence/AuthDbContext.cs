using BIP.InternalCRM.Persistence.Attributes;
using BIP.InternalCRM.WebIdentity.Roles;
using BIP.InternalCRM.WebIdentity.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BIP.InternalCRM.WebIdentity.Persistence;

public class AuthDbContext :
    IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, IdentityUserRole<Guid>, IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
{
    public AuthDbContext()
    {
    }

    public AuthDbContext(DbContextOptions<AuthDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.UseSqlServer(opts => { opts.MigrationsHistoryTable("_EFMigrations", "auth"); });
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(
            WebIdentityProject.AssemblyRef,
            typeof(AuthDbContext));

        builder.HasDefaultSchema("auth");
    }
}