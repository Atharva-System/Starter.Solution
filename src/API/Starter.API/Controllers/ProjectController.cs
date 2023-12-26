using MediatR;
using Microsoft.AspNetCore.Mvc;
using Starter.Application.Features.Common;
using Starter.Application.Features.Projects.Command.CreateCommand;
using Starter.Identity.Authorizations;
using Starter.Identity.Authorizations.Permissions;
using Action = Starter.Identity.Authorizations.Action;
using Starter.Application.Features.Projects.Query.GetProject;
using Starter.Application.Features.Projects.Dtos;
using Starter.Application.Contracts.Responses;

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

    [HttpPost("Search")]
    [MustHavePermission(Action.Search, Resource.Project)]
    public async Task<IPagedDataResponse<ProjectListDto>> GetListAsync(ISender sender, ListProjectQueryReqeust request)
    {
        return await sender.Send(request);
    }
}
