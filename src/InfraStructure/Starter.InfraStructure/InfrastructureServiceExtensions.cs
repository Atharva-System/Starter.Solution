using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Starter.Application.Contracts.Application;
using Starter.Application.Interfaces;
using Starter.InfraStructure.BackgroundJobs;
using Starter.InfraStructure.Mailing;
using Starter.InfraStructure.Services;

namespace Starter.InfraStructure
{
    public static class InfrastructureServiceExtensions
    {
        public static void AddInfrastructureSharedServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
           .AddServices()
           .AddMailing(configuration)
           .AddBackgroundJobs(configuration);
            services.AddScoped<IDateTimeService, DateTimeService>();
        }

        internal static IServiceCollection AddServices(this IServiceCollection services) =>
        services
            .AddServices(typeof(ITransientService), ServiceLifetime.Transient)
            .AddServices(typeof(IScopedService), ServiceLifetime.Scoped);
        

        internal static IServiceCollection AddServices(this IServiceCollection services, Type interfaceType, ServiceLifetime lifetime)
        {
            var interfaceTypes =
                AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(t => interfaceType.IsAssignableFrom(t)
                                && t.IsClass && !t.IsAbstract)
                    .Select(t => new
                    {
                        Service = t.GetInterfaces().FirstOrDefault(),
                        Implementation = t
                    })
                    .Where(t => t.Service is not null
                                && interfaceType.IsAssignableFrom(t.Service));

            foreach (var type in interfaceTypes)
            {
                services.AddService(type.Service!, type.Implementation, lifetime);
            }

            return services;
        }

        internal static IServiceCollection AddService(this IServiceCollection services, Type serviceType, Type implementationType, ServiceLifetime lifetime) =>
        lifetime switch
        {
            ServiceLifetime.Transient => services.AddTransient(serviceType, implementationType),
            ServiceLifetime.Scoped => services.AddScoped(serviceType, implementationType),
            ServiceLifetime.Singleton => services.AddSingleton(serviceType, implementationType),
            _ => throw new ArgumentException("Invalid lifeTime", nameof(lifetime))
        };

        public static IApplicationBuilder UseHangfire(this IApplicationBuilder builder, IConfiguration config)
        {
            return builder
                .UseHangfireDashboard(config);
        }
    }
}
