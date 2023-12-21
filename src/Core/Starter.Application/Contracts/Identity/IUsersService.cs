﻿using Starter.Application.Contracts.Responses;
using Starter.Application.Features.Common;
using Starter.Application.Models.Users;
using Starter.Application.Features.Users.Invite;

namespace Starter.Application.Contracts.Identity;
public interface IUsersService : ITransientService
{
    Task<ApiResponse<UserDetailsDto>> GetUserDetailsAsync(string userId, CancellationToken cancellationToken);
    Task<IPagedDataResponse<UserListDto>> SearchAsync(UserListFilter filter, CancellationToken cancellationToken);
    Task<ApiResponse<string>> UpdateAsync (UpdateUserDto request);
    Task<ApiResponse<string>> DeleteAsync (string userId);
    Task<ApiResponse<string>> CreateInvitationAsync(CreateUserInvitation request, string origin);
    Task<bool> ExistsUserWithEmailAsync(string email);
}
