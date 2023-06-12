using BIP.InternalCRM.Application.FileStorage;
using Microsoft.Extensions.DependencyInjection;

namespace BIP.InternalCRM.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection RegisterInfrastructureServices(
        this IServiceCollection services)
    {
        services
            .AddTransient<IFileStorageService, FileStorageService>()
                .AddOptions<FileStorageOptions>()
                .BindConfiguration(nameof(FileStorageOptions))
                .ValidateDataAnnotations();
        
        return services;
    }
}
