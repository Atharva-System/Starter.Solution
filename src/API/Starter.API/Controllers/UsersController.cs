using MediatR;
using System;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Cmp;
using Starter.Application.Contracts.Identity;
using Starter.Application.Contracts.Responses;
using Starter.Application.Features.Common;
using Starter.Application.Models.Users;
using Starter.Identity.Authorizations;
using Starter.Identity.Authorizations.Permissions;
using Action = Starter.Identity.Authorizations.Action;
using Microsoft.Extensions.Configuration;
using Starter.InfraStructure.Cors;
using Starter.Application.Features.Users.Invite;

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
}
