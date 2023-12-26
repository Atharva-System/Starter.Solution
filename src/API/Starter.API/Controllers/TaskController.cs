using MediatR;
using Microsoft.AspNetCore.Mvc;
using Starter.Application.Features.Common;
using Starter.Application.Features.Tasks.Command;
using Starter.Application.Features.Tasks.Dto;
using Starter.Application.Features.Tasks.Query;
using Starter.Identity.Authorizations;
using Starter.Identity.Authorizations.Permissions;
using Action = Starter.Identity.Authorizations.Action;

namespace Starter.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TaskController : ControllerBase
{
    [HttpPost("Create")]
    [MustHavePermission(Action.Create, Resource.Task)]
    public async Task<ApiResponse<int>> CreateTask(ISender sender, CreateTaskCommandRequest request)
    {
        return await sender.Send(request);
    }

    [HttpGet("{id}")]
    [MustHavePermission(Action.View, Resource.Task)]
    public async Task<ApiResponse<TaskDetailsDto>> GetTaskDetails(ISender sender, Guid id)
    {
        return await sender.Send(new GetTaskDetailsQueryRequest(id));
    }
}
