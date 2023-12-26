using Starter.Application.Contracts.Persistence.Repositoris.Base;
using Starter.Domain.Common;
using Starter.Application.Contracts.Persistence.Repositoris.Queries;

namespace Starter.Application.UnitOfWork;

public interface IQueryUnitOfWork
{
    // ITodoQueryRepository TodoQueryRepository { get; }

    IQueryRepository<TEntity> QueryRepository<TEntity>() where TEntity : BaseEntity, new();

    IProjectQueryRepository ProjectQuery { get; }
}
