using System.Collections;
using MediatR;
using Starter.Application.Contracts.Persistence.Repositoris.Base;
using Starter.Application.UnitOfWork;
using Starter.Domain.Common;
using Starter.Persistence.Database;
using Starter.Persistence.Repositories.Base;

namespace Starter.Persistence.UnitofWork;

public class CommandUnitOfWork : ICommandUnitOfWork
{
    private readonly AppDbContext _appDbContext;
    private Hashtable _repositories;
    private readonly IPublisher _publisher;


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public CommandUnitOfWork(AppDbContext appDbContext, IPublisher publisher)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        _appDbContext = appDbContext;
        _publisher = publisher;
    }

    //public TodoCommandRepository _todoCommandRepository;

    //public ITodoCommandRepository TodoCommandRepository => _todoCommandRepository ?? (_todoCommandRepository = new TodoCommandRepository(_appDbContext));

    public async Task<int> SaveAsync(CancellationToken cancellationToken)
    {
        var result = await _appDbContext.SaveChangesAsync(cancellationToken);
        await SendDomainEventsAsync();
        return result;
    }

    private async Task SendDomainEventsAsync()
    {
        var domainEvents = _appDbContext.ChangeTracker.Entries<BaseEntity>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .SelectMany(x => x.DomainEvents);

        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent);
        }
    }

    public ICommandRepository<TEntity> CommandRepository<TEntity>() where TEntity : BaseEntity, new()
    {
        if (_repositories == null) _repositories = new Hashtable();

        var type = typeof(TEntity).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(CommandRepository<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _appDbContext);

            _repositories.Add(type, repositoryInstance);
        }
        // Ensure _repositories[type] is not null before returning
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        return (ICommandRepository<TEntity>)_repositories[type] ?? new CommandRepository<TEntity>(_appDbContext);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
    }

    public void Dispose()
    {
        _appDbContext.Dispose();
    }
}
