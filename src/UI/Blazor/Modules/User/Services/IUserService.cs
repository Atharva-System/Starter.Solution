using Starter.Blazor.Core.Response;
using Starter.Blazor.Modules.Common;
using Starter.Blazor.Modules.User.Models;

namespace Starter.Blazor.Modules.User.Services;

public interface IUserService
{
    Task<List<UserlistDto>> GetUserlistsAsync(PaginationRequest param);
    Task<ApiResponse<string>> InviteUserAsync(InviteUserDto userDto);
    Task<ApiResponse<AcceptInviteDto>> GetAcceptInviteDetails(string userId);
    Task<ApiResponse<string>> AcceptInvite(UserRegisterDto userRegister);
    Task<string> UpdateUserProfileAsync(string UserId, UpdateProfileDto userDto);
    Task<UpdateProfileDto> GetProfileDetailAsync();
    Task<ApiResponse<string>> DeleteUser(UserlistDto user);
    Task<UserlistDto> GetUserDetailsByIdAsync(string userId);
    Task<ApiResponse<string>> UpdateUserAsync(UserlistDto userDto);
}
