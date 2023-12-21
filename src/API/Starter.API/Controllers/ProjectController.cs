using MediatR;
using Microsoft.AspNetCore.Mvc;
using Starter.Application.Features.Common;
using Starter.Application.Features.Projects.Create;
using Starter.Identity.Authorizations;
using Starter.Identity.Authorizations.Permissions;
using Action = Starter.Identity.Authorizations.Action;

namespace Starter.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectController : ControllerBase
{
    [HttpPost("Create")]
    [MustHavePermission(Action.Create, Resource.Project)]
    public async Task<ApiResponse<int>> CreateProject(ISender sender, CreateProjectCommandRequest request)
    {
        return await sender.Send(request);
    }
}
