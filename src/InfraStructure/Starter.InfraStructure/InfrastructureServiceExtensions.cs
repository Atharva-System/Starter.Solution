using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Starter.Application.Contracts.Application;
using Starter.InfraStructure.Services;

namespace Starter.InfraStructure
{
    public static class InfrastructureServiceExtensions
    {
        public static void AddInfrastructureSharedServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDateTimeService, DateTimeService>();
        }
    }


}
