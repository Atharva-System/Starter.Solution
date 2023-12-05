using Starter.Application.Contracts.Persistence.Repositoris.Base;
using Starter.Domain.Entities;

namespace Starter.Application.Contracts.Persistence.Repositoris.TodoRepository
{
    public interface ITodoCommandRepository : ICommandRepository<TodoItem>
    {
    }
}
