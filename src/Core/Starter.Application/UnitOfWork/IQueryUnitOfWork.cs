using Starter.Application.Contracts.Persistence.Repositoris.TodoRepository;

namespace Starter.Application.UnitOfWork;

public interface IQueryUnitOfWork
{
    ITodoQueryRepository TodoQueryRepository { get; }
}
