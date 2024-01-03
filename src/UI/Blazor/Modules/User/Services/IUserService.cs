using Starter.Blazor.Modules.Common;
using Starter.Blazor.Modules.User.Models;

namespace Starter.Blazor.Modules.User.Services;

public interface IUserService
{
    Task<List<UserlistDto>> GetUserlistsAsync(PaginationRequest param);
    Task<string> InviteUserAsync(InviteUserDto userDto);
    Task<string> UpdateUserProfileAsync(string UserId, UpdateProfileDto userDto);
    Task<UpdateProfileDto> GetProfileDetailAsync();
}
