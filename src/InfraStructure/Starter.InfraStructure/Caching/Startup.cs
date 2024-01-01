using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using StackExchange.Redis;
using Starter.Application.Contracts.Caching;

namespace Starter.InfraStructure.Caching;
internal static class Startup
{
    internal static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration config)
    {
        var settings = config.GetSection(nameof(CacheSettings)).Get<CacheSettings>();
        if (settings == null) return services;
        if (settings.UseDistributedCache)
        {
            if (settings.PreferRedis)
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = settings.RedisURL;
                    options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
                    {
                        AbortOnConnectFail = true,
                        EndPoints = { settings.RedisURL }
                    };
                });

                try
                {
                    Log.Information($"Connecting redis with URL {settings.RedisURL}");
                    ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(
                    new ConfigurationOptions
                    {
                        EndPoints = { settings.RedisURL },
                        AbortOnConnectFail = false,
                    });

                    var db = redis.GetDatabase();
                    var pong = db.Ping();
                    Log.Information($"Is Redis Connected : {db.IsConnected("Test")}");
                    Log.Information($"Redis Ping : {pong}");
                }
                catch (Exception ex)
                {
                    Log.Information($"Redis connection Issue : {ex.ToString()}");
                }
            }
            else
            {
                services.AddDistributedMemoryCache();
            }
            services.AddTransient<ICacheService, DistributedCacheService>();
        }
        else
        {
            services.AddTransient<ICacheService, LocalCacheService>();
        }

        services.AddMemoryCache();
        return services;
    }
}
