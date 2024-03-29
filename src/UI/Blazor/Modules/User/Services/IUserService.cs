﻿using Starter.Blazor.Core.Response;
using Starter.Blazor.Modules.Common;
using Starter.Blazor.Modules.User.Models;
using Starter.Blazor.Shared.Response;

namespace Starter.Blazor.Modules.User.Services;

public interface IUserService
{
    Task<PagedDataResponse<List<UserlistDto>>> GetUserlistsAsync(PaginationRequest param);
    Task<ApiResponse<string>> InviteUserAsync(InviteUserDto userDto);
    Task<ApiResponse<AcceptInviteDto>> GetAcceptInviteDetails(string userId);
    Task<ApiResponse<string>> AcceptInvite(UserRegisterDto userRegister);
    Task<ApiResponse<string>> UpdateUserProfileAsync(UpdateProfileDto userDto);
    Task<ApiResponse<UpdateProfileDto>> GetProfileDetailAsync();
    Task<ApiResponse<string>> DeleteUser(string id);
    Task<ApiResponse<UserlistDto>> GetUserDetailsByIdAsync(string userId);
    Task<ApiResponse<string>> UpdateUserAsync(UserlistDto userDto);
}
