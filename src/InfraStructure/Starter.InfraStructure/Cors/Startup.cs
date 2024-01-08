using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Starter.InfraStructure.Cors;
internal static class Startup
{
    private const string CorsPolicy = nameof(CorsPolicy);

    internal static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration config)
    {
        var corsSettings = config.GetSection(nameof(CorsSettings)).Get<CorsSettings>();
        if (corsSettings == null) return services;

        if (corsSettings.CorsURLs is not null && !string.IsNullOrEmpty(corsSettings.CorsURLs))
        {
            var origins = new List<string>();
            origins.AddRange(corsSettings.CorsURLs.Split(';', StringSplitOptions.RemoveEmptyEntries));
            return services.AddCors(opt =>
                opt.AddPolicy(CorsPolicy, policy =>
                policy.AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials()
                   .WithOrigins(origins.ToArray())));
        }
        else
        {
            return services.AddCors(opt =>
               opt.AddPolicy(CorsPolicy, policy =>
               policy
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                  ));
        }

    }

    internal static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app) =>
        app.UseCors(CorsPolicy);
}
