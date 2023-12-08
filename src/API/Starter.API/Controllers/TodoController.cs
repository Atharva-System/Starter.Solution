using MediatR;
using Microsoft.AspNetCore.Mvc;
using Starter.Application.Features.Todos.Create;
using Starter.Identity.Authorizations;
using Starter.Identity.Authorizations.Permissions;
using Action = Starter.Identity.Authorizations.Action;

namespace Starter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    //[Authorize(Policy = Policies.CanPurge)]
    //[Authorize(Policy = "ResourceOperationCreate")]

    public class TodoController : ControllerBase
    {
        [HttpPost("Create")]
        [MustHavePermission(Action.Create, Resource.Todo)]
        public async Task<int> CreateTodoItem(ISender sender, CreateTodoItemCommand command)
        {
            return await sender.Send(command);
        }
    }
}
