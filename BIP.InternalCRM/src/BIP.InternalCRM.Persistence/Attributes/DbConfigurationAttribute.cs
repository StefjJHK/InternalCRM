using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BIP.InternalCRM.Persistence.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class DbConfigurationAttribute : Attribute
{
    public Type[] DbContexts { get; init; }

    public DbConfigurationAttribute(params Type[] _dbContext)
    {
        DbContexts = _dbContext;
    }
}

public static class ModelBuilderExtensions
{
    public static ModelBuilder ApplyConfigurationsFromAssembly(
        this ModelBuilder builder,
        Assembly assemblyRef,
        params Type[] dbContextFilters)
    {
        builder.ApplyConfigurationsFromAssembly(
            assemblyRef,
            t =>
            {
                var isAssignable = t.GetInterfaces()
                    .Where(i => i.IsGenericType)
                    .Where(gi => gi.GetGenericTypeDefinition()
                                    .IsAssignableTo(typeof(IEntityTypeConfiguration<>)));

                if (!isAssignable.Any())
                {
                    return false;
                }

                var dbConfigAttr = t.GetCustomAttribute<DbConfigurationAttribute>();

                if (dbConfigAttr is null)
                {
                    throw new NullReferenceException($"Class {t.FullName} doesn't have {nameof(DbConfigurationAttribute)}.");
                }

                return dbConfigAttr.DbContexts
                    .Intersect(dbContextFilters)
                    .Any();
            });

        return builder;
    }
}
