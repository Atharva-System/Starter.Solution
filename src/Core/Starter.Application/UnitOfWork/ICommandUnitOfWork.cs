using Starter.Application.Contracts.Persistence.Repositoris.Base;
using Starter.Domain.Common;

namespace Starter.Application.UnitOfWork;

public interface ICommandUnitOfWork : IDisposable
{
    //ITodoCommandRepository TodoCommandRepository { get; }

    ICommandRepository<TEntity> CommandRepository<TEntity>() where TEntity : BaseEntity, new();

    Task<int> SaveAsync(CancellationToken cancellationToken);
}
