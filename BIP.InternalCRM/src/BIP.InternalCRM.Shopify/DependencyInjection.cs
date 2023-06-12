using BIP.InternalCRM.Shopify.AccessService;
using BIP.InternalCRM.Shopify.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BIP.InternalCRM.Shopify;

public static class DependencyInjection
{
    public static IServiceCollection RegisterShopifyServices(
        this IServiceCollection services,
        IConfiguration config)
    {
        services.AddOptions<ShopifyOptions>()
            .BindConfiguration(nameof(ShopifyOptions))
            .ValidateDataAnnotations();

        services.AddHttpClient(nameof(ShopifyHttpClient));
        services
            .AddTransient<IShopifyHttpClient, ShopifyHttpClient>()
            .AddTransient<IShopifyAccessService, ShopifyAccessService>();

        services.AddAutoMapper(ShopifyProject.AssemblyRef);

        services.AddMediatR(opts =>
        {
            opts.RegisterServicesFromAssembly(ShopifyProject.AssemblyRef);
        });

        return services;
    }
}
