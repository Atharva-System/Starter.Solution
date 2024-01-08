using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Starter.Application.Contracts.Application;
using Starter.Application.Contracts.Identity;
using Starter.Application.Contracts.Responses;
using Starter.Application.Exceptions;
using Starter.Application.Features.Common;
using Starter.Application.Models.Users;
using Starter.Identity.Database;
using Starter.Identity.Models;
using Starter.Application.Interfaces;
using Starter.Application.Contracts.Mailing;
using Starter.Application.Contracts.Persistence.Services;
using Starter.Application.Contracts.Caching;
using Starter.Application.Features.Users.Profile;

namespace Starter.Identity.Services;
public partial class UsersService(UserManager<ApplicationUser> userManager,
                                  RoleManager<ApplicationRole> roleManager,
                                  AppIdentityDbContext db,
                                  ICurrentUserService currentUserService,
                                  IConfiguration configuration,
                                  IEmailTemplateService templateService,
                                  IMailService mailService,
                                  IJobService jobService,
                                  ITaskService taskService,
                                  ICacheService cache,
                                  ICacheKeyService cacheKey) : IUsersService
{
    private readonly AppIdentityDbContext _db = db;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IConfiguration _configuration = configuration;
    private readonly IJobService _jobService = jobService;
    private readonly IMailService _mailService = mailService;
    private readonly IEmailTemplateService _templateService = templateService;
    private readonly ITaskService _taskService = taskService;
    private readonly ICacheService _cache = cache;
    private readonly ICacheKeyService _cacheKey = cacheKey;


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
        var usersList = (from u in _db.Users.AsNoTracking()
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
                             Status = u.IsInvitationAccepted == false ? UserStatus.Invited.ToString() : (u.IsActive ? UserStatus.Active.ToString() : UserStatus.Inactive.ToString()),
                             RoleId = r.Id,
                             Role = r.Name ?? string.Empty,
                             CreatedOn = u.CreatedOn
                         }
                    );

        var spec = new GetSearchUserRequestSpec(filter);

        var users = await usersList.ApplySpecification(spec);

        int count = await usersList.ApplySpecificationCount(spec);

        return new PagedApiResponse<UserListDto>(count, filter.PageNumber, filter.PageSize) { Data = users };
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

    public async Task<ApiResponse<string>> DeleteAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException("UserId ", userId);

        if (user.IsSuperAdmin == true && user.Email == _configuration["AppSettings:UserEmail"])
        {
            throw new Exception($"Not allowed to deleted member.");
        }

        //Check for any task assigned to user
        var userTask = await _taskService.IsTaskAssignedToUser(userId);

        if (userTask)
        {
            throw new Exception($"Cannot delete as Task is assigned to user.");
        }

        user.NormalizedUserName = user.UserName = user.UserName + "_" + Guid.NewGuid().ToString();
        var result = await _userManager.DeleteAsync(user);

        return new ApiResponse<string>
        {
            Success = result.Succeeded,
            Data = "User deleted successfully.",
            StatusCode = result.Succeeded ? HttpStatusCodes.OK : HttpStatusCodes.BadRequest,
            Message = result.Succeeded ? $"User {ConstantMessages.DeletedSuccessfully}" : $"{ConstantMessages.FailedToCreate} user."
        };
    }

    public async Task<bool> ExistsUserWithEmailAsync(string email)
    {
        bool userExists = await _db.Users
                                    .AsNoTracking()
                                    .AnyAsync(x => x.NormalizedEmail == email.ToUpper());

        return userExists;
    }

    public async Task<UserDetailsTaskDto> GetUserDetailsForTaskAsync(string userId, CancellationToken cancellationToken)
    {
        var userDto = new UserDetailsTaskDto();
        var user = await _db.Users.AsNoTracking().Where(x => x.Id == userId).FirstOrDefaultAsync();

        if (user != null)
        {
            userDto.Id = user.Id;
            userDto.FullName = user.FirstName + " " + user.LastName;
        }

        return userDto;
    }
    public async Task<ApiResponse<string>> UpdateProfileAsync(UpdateProfileRequest request)
    {
        // Retrieve the existing user
        var existingUser = await _userManager.FindByIdAsync(request.Id);

        if (existingUser == null)
        {
            return new ApiResponse<string> { Success = false, Data = "User not found", StatusCode = HttpStatusCodes.NotFound };
        }

        // Update user profile properties
        existingUser.FirstName = request.FirstName;
        existingUser.LastName = request.LastName;
        existingUser.Email = request.Email;
        existingUser.ImageUrl = request.ImageUrl;

        var result = await _userManager.UpdateAsync(existingUser);

        return new ApiResponse<string>
        {
            Success = result.Succeeded,
            Data = "Profile updated successfully.",
            StatusCode = result.Succeeded ? HttpStatusCodes.OK : HttpStatusCodes.BadRequest,
            Message = result.Succeeded ? $"User {ConstantMessages.UpdatedSuccessfully}" : $"{ConstantMessages.FailedToUpdate} user profile."
        };
    }

    public async Task<ApiResponse<UserProfileDto>> GetProfileDetailAsync()
    {
        var userDetail = await _userManager.FindByIdAsync(_currentUserService.UserId);

        if (userDetail == null)
        {
            throw new NotFoundException("User not found", _currentUserService.UserId);
        }

        var profileDetail = new UserProfileDto()
        {
            Id = userDetail.Id,
            Email = userDetail.Email,
            ImageUrl = userDetail.ImageUrl,
            FirstName = userDetail.FirstName,
            LastName = userDetail.LastName
        };

        return new ApiResponse<UserProfileDto>
        {
            Success = true,
            Data = profileDetail,
            StatusCode = HttpStatusCodes.OK
        };
    }
}
