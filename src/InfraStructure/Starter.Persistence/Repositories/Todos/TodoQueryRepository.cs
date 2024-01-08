using Microsoft.EntityFrameworkCore;
using Starter.Application.Contracts.Persistence.Repositoris.TodoRepository;
using Starter.Domain.Entities;
using Starter.Persistence.Database;
using Starter.Persistence.Repositories.Base;

namespace Starter.Persistence.Repositories.Todos;

public class TodoQueryRepository : QueryRepository<TodoItem>, ITodoQueryRepository
{
    public TodoQueryRepository(AppDbContext context) : base(context)
    {

    }

    public async Task<TodoItem> GetByGUIdAsync(Guid id)
    {
#pragma warning disable CS8603 // Possible null reference return.
        return await context.TodoItems.SingleOrDefaultAsync(x => x.Id == id);
#pragma warning restore CS8603 // Possible null reference return.
    }
}
