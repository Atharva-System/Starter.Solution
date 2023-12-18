using Microsoft.EntityFrameworkCore;
using Starter.Application.Contracts.Identity;
using Starter.Application.Exceptions;
using Starter.Application.Features.Common;
using Starter.Application.Models.Users;
using Starter.Identity.Database;

namespace Starter.Identity.Services;
public class UsersService(AppIdentityDbContext db) : IUsersService
{
    private readonly AppIdentityDbContext _db = db;
    public async Task<ApiResponse<UserDetailsDto>> GetUserDetailsAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await (from u in _db.Users.AsNoTracking()
                           join ur in _db.UserRoles.AsNoTracking() on u.Id equals ur.UserId
                           join r in _db.Roles.AsNoTracking() on ur.RoleId equals r.Id
                           where u.Id == userId
                           select new UserDetailsDto()
                           {
                               Id = u.Id,
                               FirstName = u.FirstName ?? string.Empty,
                               LastName = u.LastName ?? string.Empty,
                               Email = u.Email ?? string.Empty,
                               RoleId = r.Id,
                               RoleName = r.Name ?? string.Empty,
                           }).FirstOrDefaultAsync();

        _ = user ?? throw new NotFoundException("User ", userId);

        var response = new ApiResponse<UserDetailsDto>
        {
            Success = user != null,
            StatusCode = user != null? HttpStatusCodes.OK : HttpStatusCodes.BadRequest,
            Data = user,
            Message = user != null ? $"User {ConstantMessages.DataFound}" : $"{ConstantMessages.NotFound} user."
        };
        return response;
    }
}
