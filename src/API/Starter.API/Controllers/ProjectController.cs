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
using Starter.Application.Features.Tasks.Dto;
using Starter.Application.Features.Tasks.Query;
using MediatR;
using Starter.Application.Features.Projects.Query.GetProjectDetails;
using Starter.Application.Features.Projects.Command;
using Microsoft.OpenApi.Validations.Rules;
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

    [HttpGet("{id:guid}")]
    [MustHavePermission(Action.View, Resource.Project)]
    public async Task<ApiResponse<ProjectDto>> GetAsync(Guid id)
    {
        return await Mediator.Send(new GetProjectDetailsQueryRequest(id));
    }

    [HttpDelete("{id}")]
    [MustHavePermission(Action.Delete, Resource.Project)]
    public async Task<ApiResponse<string>> DeleteProject(Guid id)
    {
        return await Mediator.Send(new DeleteProjectCommandRequest(id));
    }


    [HttpPut("{id}")]
    [MustHavePermission(Action.Update, Resource.Project)]
    public async Task<ApiResponse<string>> UpdateProject(Guid id, UpdateProjectCommand request)
    {
        if (id != request.Id)
        {
            return new ApiResponse<string>
            {
                Success = false,
                Data = "The provided ID in the route does not match the ID in the request body.",
                StatusCode = HttpStatusCodes.BadRequest
            };
        }
        return await Mediator.Send(request);
    }
}
