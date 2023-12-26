using Microsoft.AspNetCore.Mvc;
using Starter.Application.Contracts.Identity;
using Starter.Application.Contracts.Responses;
using Starter.Application.Features.Common;
using Starter.Application.Models.Users;
using Starter.Identity.Authorizations;
using Starter.Identity.Authorizations.Permissions;
using Action = Starter.Identity.Authorizations.Action;
using MediatR;
using Starter.InfraStructure.Cors;
using Starter.Application.Features.Users.Invite;
using Microsoft.AspNetCore.Authorization;
using Starter.Application.Features.Users.AcceptInvite;

namespace Starter.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IUsersService userService, IConfiguration configuration) : ControllerBase
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
    public async Task<ApiResponse<string>> InviteAsync(ISender sender, CreateUserInvitation request)
    {
        return await sender.Send(new CreateUserInvitationRequest() { request = request, Origion = GetOriginFromRequest() });
    }

    private string GetOriginFromRequest()
    {
        var corsSettings = _configuration.GetSection(nameof(CorsSettings)).Get<CorsSettings>();

        if (corsSettings != null && corsSettings.Angular is not null)
        {
            return corsSettings.Angular;
        }
        return $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}";
    }

    [AllowAnonymous]
    [HttpPost("accept-invite")]
    public async Task<ApiResponse<string>> AcceptInviteAsync(ISender sender, AcceptUserInvitationRequest request)
    {
        return await sender.Send(request);
    }
}
