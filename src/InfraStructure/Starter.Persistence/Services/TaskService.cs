using Microsoft.EntityFrameworkCore;
using Starter.Application.Contracts.Persistence.Services;
using Starter.Persistence.Database;

namespace Starter.Persistence.Services;
public class TaskService(AppDbContext context) : ITaskService
{
    private readonly AppDbContext _context = context;

    public async Task<bool> IsTaskAssignedToUser(string userId)
    {
         return await _context.Tasks.AnyAsync(x => x.AssignedTo == userId);
    }

    public async Task<bool> IsTaskCreatedForProject(Guid projectId)
    {
        return await _context.Tasks.AnyAsync(x => x.ProjectId == projectId);
    }
}
