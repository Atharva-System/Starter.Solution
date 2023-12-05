using Starter.Application.Contracts.Persistence.Repositoris.Base;
using Starter.Domain.Common;
using Starter.Persistence.Database;

namespace Starter.Persistence.Repositories.Base;

public class CommandRepository<T>(AppDbContext context) : ICommandRepository<T> where T : BaseEntity, new()
{

    private readonly AppDbContext _appDbContext = context;

    public async Task<T> AddAsync(T entity)
    {
        await _appDbContext.Set<T>().AddAsync(entity);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _appDbContext.Set<T>().AddRangeAsync(entities);
    }

    public void Remove(T entity)
    {
        _appDbContext.Set<T>().Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        _appDbContext.Set<T>().RemoveRange(entities);
    }

    public T Update(T entity)
    {
        _appDbContext.Set<T>().Update(entity);
        return entity;
    }

}
