using BIP.InternalCRM.Application.Subscriptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BIP.InternalCRM.Application;

public static class DependencyInjection
{
    public static IServiceCollection RegisterApplicationServices(
        this IServiceCollection services,
        IConfiguration config)
    {
        services
            .AddOptions<SubscriptionNumberOptions>()
            .BindConfiguration(nameof(SubscriptionNumberOptions))
            .ValidateDataAnnotations();
        
        return services;
    }
}