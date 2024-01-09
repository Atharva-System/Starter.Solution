using Microsoft.EntityFrameworkCore;
using Starter.Application.Contracts.Persistence.Services;
using Starter.Application.Features.Common;
using Starter.Application.Features.Tasks.Dto;
using Starter.Identity.Database;

namespace Starter.Identity.Services;
public class UsersPersistenceService(AppIdentityDbContext db) : IUsersPersistenceService
{
    private readonly AppIdentityDbContext _db = db;

    public async Task<ApiResponse<List<TaskAssigneeDto>>> GetAssigneeForTaskAsync()
    {
        var users = await (from u in _db.Users.AsNoTracking()
                           join ur in _db.UserRoles.AsNoTracking() on u.Id equals ur.UserId
                           join r in _db.Roles.AsNoTracking() on ur.RoleId equals r.Id
                           where r.NormalizedName == "USER"
                           select new TaskAssigneeDto
                           {
                               Id = u.Id,
                               Name = u.FirstName + " " + u.LastName,
                           }).ToListAsync();

        var response = new ApiResponse<List<TaskAssigneeDto>>
        {
            Success = users.Any(),
            StatusCode = users?.Any() == true ? HttpStatusCodes.OK : HttpStatusCodes.BadRequest,
            Data = users,
        };
        return response;
    }
}
