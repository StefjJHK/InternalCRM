using BIP.InternalCRM.Application;
using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Application.FileStorage;
using BIP.InternalCRM.Application.Products;
using BIP.InternalCRM.Application.Subscriptions;
using BIP.InternalCRM.Persistence.Domain;
using BIP.InternalCRM.Persistence.Domain.Interceptors;
using BIP.InternalCRM.Persistence.Domain.Invoices;
using BIP.InternalCRM.Persistence.Domain.Products;
using BIP.InternalCRM.Shopify;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BIP.InternalCRM.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection RegisterPersistenceServices(
        this IServiceCollection services,
        IConfiguration config)
    {
        services
            .AddSingleton<DomainEventInterceptors>()
            .AddDbContext<DomainDbContext>((provider, opts) =>
            {
                opts.UseSqlServer(config.GetConnectionString(DbConstants.DbConnectionString));
                opts.AddInterceptors(provider.GetRequiredService<DomainEventInterceptors>());
            })
            .AddTransient<IDomainDbContext>(_ => _.GetRequiredService<DomainDbContext>())
            .AddTransient<IRelationsDbContext>(_ => _.GetRequiredService<DomainDbContext>())
            .AddTransient<IShopifyDbContext>(_ => _.GetRequiredService<DomainDbContext>())
            .AddTransient<IUnitOfWork>(_ => _.GetRequiredService<DomainDbContext>());

        services.AddAutoMapper(PersistenceProject.AssemblyRef);

        services
            .AddTransient<IIntelliLockProjectRepository, IntelliLockProjectRepository>(
                provider => new IntelliLockProjectRepository(
                    FileSystemPaths.IntelliLockProjectsPath,
                    provider.GetRequiredService<IFileStorageService>(),
                    provider.GetRequiredService<IDomainDbContext>()))
            .AddTransient<IIntelliLockLicenseRepository, IntelliLockLicenseRepository>(
                provider => new IntelliLockLicenseRepository(
                    FileSystemPaths.IntelliLockLicensesPath,
                    provider.GetRequiredService<IFileStorageService>(),
                    provider.GetRequiredService<IDomainDbContext>()))
            .AddTransient<IImageRepository, ImageRepository>(
                provider => new ImageRepository(
                    FileSystemPaths.ImagesPath,
                    provider.GetRequiredService<IFileStorageService>(),
                    provider.GetRequiredService<IDomainDbContext>()));

        return services;
    }
}
