﻿using Starter.Blazor.Core.Response;
using Starter.Blazor.Modules.Common;
using Starter.Blazor.Modules.User.Models;

namespace Starter.Blazor.Modules.User.Services;

public interface IUserService
{
    Task<List<UserlistDto>> GetUserlistsAsync(PaginationRequest param);
    Task<ApiResponse<string>> InviteUserAsync(InviteUserDto userDto);
}
