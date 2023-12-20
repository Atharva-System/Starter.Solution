﻿using Microsoft.AspNetCore.Mvc;
using Starter.Application.Contracts.Identity;
using Starter.Application.Contracts.Responses;
using Starter.Application.Features.Common;
using Starter.Application.Models.Users;
using Starter.Identity.Authorizations;
using Starter.Identity.Authorizations.Permissions;
using Action = Starter.Identity.Authorizations.Action;

namespace Starter.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IUsersService userService) : ControllerBase
{
    private readonly IUsersService _usersService = userService;

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
}
