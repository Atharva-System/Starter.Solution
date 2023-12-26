using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Org.BouncyCastle.Asn1.Cmp;
using Starter.Application.Contracts.Responses;
using Starter.Application.Features.Common;
using Starter.Application.Features.Projects.Command.CreateCommand;
using Starter.Application.Features.Projects.Dto;
using Starter.Application.Features.Projects.Query.GetProjectDetails;
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

    [HttpGet("{id:guid}")]
    [MustHavePermission(Action.View, Resource.Project)]
    [OpenApiOperation("Get Project Details", "")]
    public async Task<IDataResponse<ProjectDto>> GetAsync(Guid id, ISender sender)
    {
        return await sender.Send(new GetProjectDetailsQueryRequest(id));
    }
}
