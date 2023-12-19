using Microsoft.EntityFrameworkCore;
using Starter.Application.Contracts.Application;
using Starter.Application.Contracts.Identity;
using Starter.Application.Contracts.Responses;
using Starter.Application.Exceptions;
using Starter.Application.Features.Common;
using Starter.Application.Models.Users;
using Starter.Identity.Database;

namespace Starter.Identity.Services;
public partial class UsersService(AppIdentityDbContext db, ICurrentUserService currentUserService) : IUsersService
{
    private readonly AppIdentityDbContext _db = db;
    private readonly ICurrentUserService _currentUserService = currentUserService;

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
            StatusCode = user != null ? HttpStatusCodes.OK : HttpStatusCodes.BadRequest,
            Data = user,
            Message = user != null ? $"User {ConstantMessages.DataFound}" : $"{ConstantMessages.NotFound} user."
        };
        return response;
    }

    public async Task<IPagedDataResponse<UserListDto>> SearchAsync(UserListFilter filter, CancellationToken cancellationToken)
    {
        var usersList = await (from u in _db.Users.AsNoTracking()
                               join ur in _db.UserRoles.AsNoTracking() on u.Id equals ur.UserId
                               join r in _db.Roles.AsNoTracking() on ur.RoleId equals r.Id
                               where u.Id != _currentUserService.UserId
                               select new UserListDto()
                               {
                                   Id = u.Id,
                                   FirstName = u.FirstName ?? string.Empty,
                                   LastName = u.LastName ?? string.Empty,
                                   Email = u.Email ?? string.Empty,
                                   FullName = u.FirstName + " " + u.LastName,
                                   Status = u.IsInvitationAccepted == false ? "Invited" : (u.IsActive ? "Active" : "InActive"),
                                   Role = r.Name ?? string.Empty,
                                   CreatedOn = u.CreatedOn
                               }
                    ).ToListAsync<UserListDto>();

        var spec = new GetSearchUserRequestSpec(filter);

        var users = usersList.ApplySpecification(spec);

        int count = usersList.ApplySpecificationCount(spec);

        return new PagedApiResponse<UserListDto>(count, filter.PageNumber, filter.PageSize) { Data = users };
    }
}
