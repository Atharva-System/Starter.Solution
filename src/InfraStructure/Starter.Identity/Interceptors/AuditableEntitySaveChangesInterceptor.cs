using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Starter.Application.Contracts.Application;
using Starter.Domain.Common.Contracts;

namespace Starter.Identity.Interceptors;
public class AuditableEntitySaveChangesInterceptor(
        ICurrentUserService currentUserService,
        IDateTimeService dateTime) : SaveChangesInterceptor
{
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IDateTimeService _dateTime = dateTime;

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        DateTimeOffset now = _dateTime.Now;

        foreach(var entry in context.ChangeTracker.Entries<IAuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = _currentUserService.UserId;
                entry.Entity.CreatedOn = now;
            }

            if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
            {
                entry.Entity.ModifiedBy = _currentUserService.UserId;
                entry.Entity.ModifiedOn = now;
            }

            if (entry.State == EntityState.Deleted)
            {
                if (entry.Entity is ISoftDelete softDelete)
                {
                    softDelete.IsDeleted = true;
                    softDelete.DeletedBy = _currentUserService.UserId;
                    softDelete.DeletedOn = now;
                    entry.State = EntityState.Modified;
                }
            }
        }
    }
}
