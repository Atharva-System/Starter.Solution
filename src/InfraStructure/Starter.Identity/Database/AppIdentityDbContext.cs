using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Starter.Domain.Common;
using Starter.Domain.Common.Contracts;
using Starter.Domain.Entities;
using Starter.Identity.Interceptors;
using Starter.Identity.Models;

namespace Starter.Identity.Database;

public class AppIdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>, IdentityUserRole<string>, ApplicationUserLogin, ApplicationRoleClaim, IdentityUserToken<string>>
{
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options,
            AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) : base(options)
    {
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }
    public DbSet<Trail> AuditTrails => Set<Trail>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // QueryFilters need to be applied before base.OnModelCreating
        builder.AppendGlobalQueryFilter<ISoftDelete>(s => s.IsDeleted == false);

        builder.HasDefaultSchema(SchemaNames.Identity);
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
