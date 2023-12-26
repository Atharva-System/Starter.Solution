using MediatR;
using Microsoft.AspNetCore.Mvc;
using Starter.Application.Features.Common;
using Starter.Application.Features.Tasks.CreateCommand;
using Starter.Identity.Authorizations;
using Starter.Identity.Authorizations.Permissions;
using Action = Starter.Identity.Authorizations.Action;

namespace Starter.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TaskController : Controller
{
    [HttpPost("Create")]
    [MustHavePermission(Action.Create, Resource.Task)]
    public async Task<ApiResponse<int>> CreateTask(ISender sender, CreateTaskCommandRequest request)
    {
        return await sender.Send(request);
    }
}
