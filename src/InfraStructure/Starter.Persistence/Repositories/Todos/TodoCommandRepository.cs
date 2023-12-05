using Starter.Application.Contracts.Persistence.Repositoris.TodoRepository;
using Starter.Domain.Entities;
using Starter.Persistence.Database;
using Starter.Persistence.Repositories.Base;

namespace Starter.Persistence.Repositories.Todos;

public class TodoCommandRepository : CommandRepository<TodoItem>, ITodoCommandRepository
{
    public TodoCommandRepository(AppDbContext context) : base(context)
    {
    }
}
