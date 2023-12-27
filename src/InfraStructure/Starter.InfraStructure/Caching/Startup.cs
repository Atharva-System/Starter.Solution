using Microsoft.Extensions.DependencyInjection;
using Starter.Application.Contracts.Caching;

namespace Starter.InfraStructure.Caching;
internal static class Startup
{
    internal static IServiceCollection AddCaching(this IServiceCollection services)
    {
        services.AddTransient<ICacheService, LocalCacheService>();
        services.AddMemoryCache();
        return services;
    }
}
