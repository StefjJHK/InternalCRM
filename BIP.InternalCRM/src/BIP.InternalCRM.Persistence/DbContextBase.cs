using BIP.InternalCRM.Persistence.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BIP.InternalCRM.Persistence;

public abstract class DbContextBase : DbContext
{
    private readonly string _scheme;

    protected DbContextBase(string scheme)
    {
        _scheme = scheme;
    }

    protected DbContextBase(
        string scheme,
        DbContextOptions options)
        : base(options)
    {
        _scheme = scheme;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.UseSqlServer(opts =>
        {
            opts.MigrationsHistoryTable("_EFMigrations", _scheme);
        });
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        builder
            .Properties<DateTime>()
            .HaveConversion<DateTimeToTicksConverter>();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema(_scheme);

        builder.ApplyConfigurationsFromAssembly(
            PersistenceProject.AssemblyRef,
            GetType()
        );
        
        builder.AddStronglyTypedIdConverters();
    }

}
