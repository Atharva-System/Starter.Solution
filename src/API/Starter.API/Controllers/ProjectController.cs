using Microsoft.AspNetCore.Mvc;
using Starter.API.Controllers.Base;
using Starter.Application.Features.Common;
using Starter.Application.Features.Projects.Command.CreateCommand;
using Starter.Identity.Authorizations;
using Starter.Identity.Authorizations.Permissions;
using Action = Starter.Identity.Authorizations.Action;

namespace Starter.API.Controllers;

public class ProjectController : BaseApiController
{
    [HttpPost("Create")]
    [MustHavePermission(Action.Create, Resource.Project)]
    public async Task<ApiResponse<int>> CreateProject(CreateProjectCommandRequest request)
    {
        return await Mediator.Send(request);
    }
}
