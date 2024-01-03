using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Starter.API.Controllers.Base;
using Starter.Application.Contracts.Identity;
using Starter.Application.Contracts.Responses;
using Starter.Application.Features.Common;
using Starter.Application.Features.Users.AcceptInvite;
using Starter.Application.Features.Users.Invite;
using Starter.Application.Features.Users.Profile;
using Starter.Application.Models.Users;
using Starter.Identity.Authorizations;
using Starter.Identity.Authorizations.Permissions;
using Action = Starter.Identity.Authorizations.Action;

namespace Starter.API.Controllers;

public class UsersController(IUsersService userService, IConfiguration configuration) : BaseApiController
{
    private readonly IUsersService _usersService = userService;
    private readonly IConfiguration _configuration = configuration;

    [HttpGet("{id}")]
    [MustHavePermission(Action.View, Resource.Users)]
    public async Task<ApiResponse<UserDetailsDto>> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        return await _usersService.GetUserDetailsAsync(id, cancellationToken);
    }

    [HttpPost("search")]
    [MustHavePermission(Action.Search, Resource.Users)]
    public async Task<IPagedDataResponse<UserListDto>> GetListAsync(UserListFilter filter, CancellationToken cancellationToken)
    {
        return await _usersService.SearchAsync(filter, cancellationToken);
    }

    [HttpPut("{id}")]
    [MustHavePermission(Action.Update, Resource.Users)]
    public async Task<ApiResponse<string>> UpdateAsync(string id, UpdateUserDto request)
    {
        if (id != request.Id)
        {
            return new ApiResponse<string>
            {
                Success = false,
                Data = "The provided ID in the route does not match the ID in the request body.",
                StatusCode = HttpStatusCodes.BadRequest
            };
        }
        return await _usersService.UpdateAsync(request);
    }

    [HttpDelete("{id}")]
    [MustHavePermission(Action.Delete, Resource.Users)]
    public async Task<ApiResponse<string>> DeleteAsync(string id)
    {
        return await _usersService.DeleteAsync(id);
    }

    [HttpPost("invite-user")]
    [MustHavePermission(Action.Create, Resource.Users)]
    public async Task<ApiResponse<string>> InviteAsync(CreateUserInvitation request)
    {
            return await Mediator.Send(new CreateUserInvitationRequest() { request = request, Origion = GetOriginFromRequest(_configuration) });
    }

    [HttpPut("{id}/update-profile")]
    [MustHavePermission(Action.Update, Resource.Users)]
    public async Task<ApiResponse<string>> UpdateProfileAsync(string id, UpdateProfileRequest request)
    {
        if (id != request.Id)
        {
            return new ApiResponse<string>
            {
                Success = false,
                Data = "The provided ID in the route does not match the ID in the request body.",
                StatusCode = HttpStatusCodes.BadRequest
            };
        }

        var updateProfileCommand = new UpdateProfileRequest
        {
            Id = id,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            ImageUrl = request.ImageUrl,
        };

        return await Mediator.Send(updateProfileCommand);
    }

    [AllowAnonymous]
    [HttpPost("accept-invite")]
    public async Task<ApiResponse<string>> AcceptInviteAsync(AcceptUserInvitationRequest request)
    {
        return await Mediator.Send(request);
    }

    [AllowAnonymous]
    [HttpGet("get-invite-details/{userId}")]
    public async Task<ApiResponse<UserInviteDto>> GetAcceptInviteDetailsAsync(string userId)
    {
        return await _usersService.GetAcceptInviteDetailsAsync(userId);
    }
}
