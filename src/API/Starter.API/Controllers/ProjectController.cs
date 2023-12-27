using Microsoft.AspNetCore.Mvc;
using Starter.API.Controllers.Base;
using Starter.Application.Features.Common;
using Starter.Application.Features.Projects.Command.CreateCommand;
using Starter.Identity.Authorizations;
using Starter.Identity.Authorizations.Permissions;
using Action = Starter.Identity.Authorizations.Action;
using Starter.Application.Features.Projects.Query.GetProject;
using Starter.Application.Features.Projects.Dtos;
using Starter.Application.Contracts.Responses;
using Starter.Application.Features.Projects.Command.UpdateProject;

namespace Starter.API.Controllers;

public class ProjectController : BaseApiController
{
    [HttpPost("Create")]
    [MustHavePermission(Action.Create, Resource.Project)]
    public async Task<ApiResponse<int>> CreateProject(CreateProjectCommandRequest request)
    {
        return await Mediator.Send(request);
    }

    [HttpPost("Search")]
    [MustHavePermission(Action.Search, Resource.Project)]
    public async Task<IPagedDataResponse<ProjectListDto>> GetListAsync(ListProjectQueryReqeust request)
    {
        return await Mediator.Send(request);
    }

    [HttpPost("Update")]
    [MustHavePermission(Action.Update, Resource.Project)]
    public async Task<ApiResponse<int>> UpdateProject(UpdateProjectCommand request)
    {
        return await Mediator.Send(request);
    }
}
