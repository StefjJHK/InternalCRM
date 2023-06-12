using BIP.InternalCRM.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BIP.InternalCRM.Infrastructure;

public static class CorsConfiguration
{
    public static IServiceCollection AddAppCors(this IServiceCollection services, IConfiguration config)
    {
        services.AddCors(options =>
        {
            var corsOptions = config
                .GetSection(nameof(CorsOptions))
                .Get<CorsOptions>();

            if (corsOptions is { Enabled: true })
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins(corsOptions.ClientOrigins)
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            }
        });

        return services;
    }
}