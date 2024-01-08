using Starter.Application.Contracts.Persistence.Repositoris.Base;
using Starter.Application.Contracts.Persistence.Repositoris.TodoRepository;
using Starter.Domain.Common;
using Starter.Application.Contracts.Persistence.Repositoris.Queries;
using Starter.Application.Contracts.Persistence.Repositoris.Task;

namespace Starter.Application.UnitOfWork;

public interface IQueryUnitOfWork : IDisposable
{
    ITodoQueryRepository TodoQuery { get; }

    IQueryRepository<TEntity> QueryRepository<TEntity>() where TEntity : BaseEntity, new();

    IProjectQueryRepository ProjectQuery { get; }
    ITaskQueryRepository _taskQueryRepository { get; }
}
