using Starter.Application.Contracts.Persistence.Repositoris.Base;
using Starter.Domain.Entities;

namespace Starter.Application.Contracts.Persistence.Repositoris.TodoRepository;

public interface ITodoQueryRepository : IQueryRepository<TodoItem>
{
    Task<TodoItem> GetByGUIdAsync(Guid id);
}
