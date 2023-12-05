using Starter.Domain.Common;
using System.Linq.Expressions;

namespace Starter.Application.Contracts.Persistence.Repositoris.Base;

public interface IQueryRepository<T> where T : BaseEntity, new()
{
    Task<IQueryable<T>> GetAllAsync(bool isChangeTracking = false);
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
    Task<IEnumerable<T>> GetAllWithIncludeAsync(bool isChangeTracking = false, Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includes);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    Task<T> GetAsync(Expression<Func<T, bool>> predicate, bool isChangeTracking = false);
    Task<T> GetWithIncludeAsync(bool isChangeTracking, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    Task<T> GetByIdAsync(string id, bool isChangeTracking = false);

    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

}
