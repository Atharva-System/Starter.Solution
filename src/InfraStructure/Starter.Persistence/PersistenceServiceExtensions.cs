using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Starter.Application.UnitOfWork;
using Starter.Persistence.Database;
using Starter.Persistence.Interceptors;
using Starter.Persistence.UnitofWork;

namespace Starter.Persistence;

public static class PersistenceServiceExtensions
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        services.AddScoped<DispatchDomainEventsInterceptor>();

        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            options.AddInterceptors(
                    sp.GetRequiredService<AuditableEntitySaveChangesInterceptor>(),
                    sp.GetRequiredService<DispatchDomainEventsInterceptor>()
                );

            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<AppDbContextInitialiser>();

        //services.AddScoped<ITodoCommandRepository, TodoCommandRepository>();

        services.AddScoped<ICommandUnitOfWork, CommandUnitOfWork>();
        services.AddScoped<IQueryUnitOfWork, QueryUnitOfWork>();


        return services;
    }
}
