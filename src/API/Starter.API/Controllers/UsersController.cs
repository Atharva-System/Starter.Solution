using Microsoft.AspNetCore.Mvc;
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
}
