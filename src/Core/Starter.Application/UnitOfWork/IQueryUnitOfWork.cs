using Starter.Application.Contracts.Persistence.Repositoris.Base;
using Starter.Domain.Common;

namespace Starter.Application.UnitOfWork;

public interface IQueryUnitOfWork
{
    // ITodoQueryRepository TodoQueryRepository { get; }

    IQueryRepository<TEntity> QueryRepository<TEntity>() where TEntity : BaseEntity, new();
}
