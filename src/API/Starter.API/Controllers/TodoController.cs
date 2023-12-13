using MediatR;
using Microsoft.AspNetCore.Mvc;
using Starter.Application.Features.Common;
using Starter.Application.Features.Todos.Create;
using Starter.Identity.Authorizations;
using Starter.Identity.Authorizations.Permissions;
using Action = Starter.Identity.Authorizations.Action;

namespace Starter.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoController : ControllerBase
{
    [HttpPost("Create")]
    [MustHavePermission(Action.Create, Resource.Todo)]
    public async Task<ApiResponse<int>> CreateTodoItem(ISender sender, CreateTodoItemCommandReqeust command)
    {
        return await sender.Send(command);
    }
}
