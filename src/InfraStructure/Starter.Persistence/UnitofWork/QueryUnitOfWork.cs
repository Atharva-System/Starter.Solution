using System.Collections;
using Microsoft.EntityFrameworkCore;
using Starter.Application.Contracts.Persistence.Repositoris.Base;
using Starter.Application.Contracts.Persistence.Repositoris.TodoRepository;
using Starter.Application.UnitOfWork;
using Starter.Domain.Common;
using Starter.Persistence.Database;
using Starter.Persistence.Repositories.Base;
using Starter.Persistence.Repositories.Todos;

namespace Starter.Persistence.UnitofWork;

public class QueryUnitOfWork : IQueryUnitOfWork
{
    private readonly AppDbContext _appDbContext;

    private Hashtable _repositories;

    public TodoQueryRepository _todoQuery;
    public ITodoQueryRepository TodoQuery => _todoQuery ?? (_todoQuery = new TodoQueryRepository(_appDbContext));


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public QueryUnitOfWork(AppDbContext appDbContext)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        _appDbContext = appDbContext;
    }

    public IQueryRepository<TEntity> QueryRepository<TEntity>() where TEntity : BaseEntity, new()
    {
        if (_repositories == null) _repositories = new Hashtable();

        var type = typeof(TEntity).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(QueryRepository<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _appDbContext);

            _repositories.Add(type, repositoryInstance);
        }
        // Ensure _repositories[type] is not null before returning
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        return (IQueryRepository<TEntity>)_repositories[type] ?? new QueryRepository<TEntity>(_appDbContext);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
    }

    public void Dispose()
    {
        _appDbContext.Dispose();
    }
}
