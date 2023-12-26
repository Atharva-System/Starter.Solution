using Microsoft.AspNetCore.Mvc;
using Starter.API.Controllers.Base;
using Starter.Application.Features.Common;
using Starter.Application.Features.Todos.Command.CreateCommand;
using Starter.Application.Features.Todos.Dto;
using Starter.Application.Features.Todos.Query.GetToDoItem;
using Starter.Identity.Authorizations;
using Starter.Identity.Authorizations.Permissions;
using Action = Starter.Identity.Authorizations.Action;

namespace Starter.API.Controllers;

public class TodoController : BaseApiController
{
    [HttpPost("Create")]
    [MustHavePermission(Action.Create, Resource.Todo)]
    public async Task<ApiResponse<int>> CreateTodoItem(CreateTodoItemCommandReqeust command)
    {
        return await Mediator.Send(command);
    }

    [HttpGet("{id}")]
    [MustHavePermission(Action.View, Resource.Todo)]
    public async Task<ApiResponse<GetToDoItemDto>> GetTodoItem(Guid id)
    {
        return await Mediator.Send(new GetToDoItemRequest(id));
    }
}
