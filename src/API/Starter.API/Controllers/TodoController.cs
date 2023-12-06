using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Starter.Application.Features.Todos.Create;

namespace Starter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    //[Authorize(Policy = Policies.CanPurge)]
    [Authorize(Policy = "ResourceOperationCreate")]

    public class TodoController : ControllerBase
    {

        [HttpPost("CreateTODO")]
        public async Task<int> CreateTodoItem(ISender sender, CreateTodoItemCommand command)
        {
            return await sender.Send(command);
        }
    }
}
