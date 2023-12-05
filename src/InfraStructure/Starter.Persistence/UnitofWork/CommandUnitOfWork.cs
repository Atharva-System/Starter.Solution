using Starter.Application.Contracts.Persistence.Repositoris.Base;
using Starter.Application.UnitOfWork;
using Starter.Domain.Common;
using Starter.Persistence.Database;
using Starter.Persistence.Repositories.Base;
using System.Collections;

namespace Starter.Persistence.UnitofWork;

public class CommandUnitOfWork : ICommandUnitOfWork
{
    private readonly AppDbContext _appDbContext;

    private Hashtable _repositories;


    public CommandUnitOfWork(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    //public TodoCommandRepository _todoCommandRepository;

    //public ITodoCommandRepository TodoCommandRepository => _todoCommandRepository ?? (_todoCommandRepository = new TodoCommandRepository(_appDbContext));

    public async Task<int> SaveAsync(CancellationToken cancellationToken)
    {
        return await _appDbContext.SaveChangesAsync(cancellationToken);
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
        return (ICommandRepository<TEntity>)_repositories[type] ?? new CommandRepository<TEntity>(_appDbContext);
    }

    public void Dispose()
    {
        _appDbContext.Dispose();
    }
}
