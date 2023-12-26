using MediatR;
using Microsoft.AspNetCore.Mvc;
using Starter.Application.Features.Common;
using Starter.Application.Features.Projects.Command.CreateCommand;
using Starter.Application.Features.Projects.Dto;
using Starter.Application.Features.Projects.Query.GetProjectDetails;
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

    [HttpGet("{id:guid}")]
    [MustHavePermission(Action.View, Resource.Project)]
    [OpenApiOperation("Get Project Details", "")]
    public async Task<IDataResponse<ProjectDto>> GetAsync(Guid id, ISender sender)
    {
        return await sender.Send(new GetProjectDetailsQueryRequest(id));
    }
}
