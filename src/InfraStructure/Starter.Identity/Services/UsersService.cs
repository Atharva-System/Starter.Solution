using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Starter.Application.Contracts.Identity;
using Starter.Application.Exceptions;
using Starter.Application.Features.Common;
using Starter.Application.Models.Users;
using Starter.Identity.Database;
using Starter.Identity.Models;

namespace Starter.Identity.Services;
public partial class UsersService(UserManager<ApplicationUser> userManager,
                                  RoleManager<ApplicationRole> roleManager,
                                  AppIdentityDbContext db) : IUsersService
{
    private readonly AppIdentityDbContext _db = db;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
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

    public async Task<ApiResponse<string>> UpdateAsync(UpdateUserDto request)
    {
        var user = await _userManager.FindByIdAsync(request.Id);

        _ = user ?? throw new NotFoundException("UserId ", request.Id);

        var existingEmail = await _userManager.FindByEmailAsync(request.Email!);

        if (existingEmail != null && existingEmail.Id != request.Id)
        {
            throw new Exception($"Email '{request.Email}' already exists.");
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;

        if (request.Email != user.Email)
        {
            user.UserName = user.Email = request.Email;
            user.NormalizedUserName = user.NormalizedEmail = request.Email!.ToUpper();
        }

        //var newRole = await _roleManager.FindByIdAsync(request.RoleId) ?? throw new NotFoundException("RoleId ", request.RoleId);

        //var currentRoles = await _userManager.GetRolesAsync(user);
        //await _userManager.RemoveFromRolesAsync(user, currentRoles);
        //await _userManager.AddToRoleAsync(user, newRole.Name);

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            throw new Exception($"Failed to update user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return new ApiResponse<string> 
        { 
            Success = result.Succeeded,
            Data = "User updated successfully.",
            StatusCode = result.Succeeded ? HttpStatusCodes.OK : HttpStatusCodes.BadRequest,
            Message = result.Succeeded ? $"User {ConstantMessages.UpdatedSuccessfully}" : $"{ConstantMessages.FailedToCreate} user."
        };
    }
}
