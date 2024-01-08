using Starter.Blazor.Modules.Common;
using Starter.Blazor.Modules.User.Models;
using Starter.Blazor.Shared.Response;

namespace Starter.Blazor.Modules.User.Services;

public interface IUserService
{
    Task<PagedDataResponse<List<UserlistDto>>> GetUserlistsAsync(PaginationRequest param);
    Task<string> InviteUserAsync(InviteUserDto userDto);
}
