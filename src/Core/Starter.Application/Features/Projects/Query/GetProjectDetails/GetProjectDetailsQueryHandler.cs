using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Localization;
using Starter.Application.Contracts.Identity;
using Starter.Application.Contracts.Responses;
using Starter.Application.Exceptions;
using Starter.Application.Features.Common;
using Starter.Application.Features.Projects.Dto;
using Starter.Application.UnitOfWork;
using Starter.Domain.Entities;
using Starter.Domain.Enums;

namespace Starter.Application.Features.Projects.Query.GetProjectDetails;
public record GetProjectDetailsQueryRequest(Guid id) : IRequest<ApiResponse<ProjectDto>>;
public class GetProjectDetailsQueryHandler : IRequestHandler<GetProjectDetailsQueryRequest,ApiResponse<ProjectDto>>
{
    private readonly IQueryUnitOfWork _query;

    public async Task<ApiResponse<ProjectDto>> Handle(GetProjectDetailsQueryRequest request, CancellationToken cancellationToken)
    {
        var project = await _query.QueryRepository<Project>().GetByIdAsync(request.id.ToString());
        _ = project ?? throw new NotFoundException("ProjectId", request.id);

        var ProjectDetailDto = new ProjectDto
        {
            Id = project.Id,
            ProjectName = project.ProjectName!,
            Description = project.Description!,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            EstimatedHours = project.EstimatedHours,
            CreatedOn = project.CreatedOn.DateTime
        };

        var response = new ApiResponse<ProjectDto>
        {
            Success = ProjectDetailDto != null,
            StatusCode = ProjectDetailDto != null ? HttpStatusCodes.OK : HttpStatusCodes.BadRequest,
            Data = ProjectDetailDto!,
            Message = ProjectDetailDto != null ? $"Project {ConstantMessages.DataFound}" : $"{ConstantMessages.NotFound} Project."
        };

        return response;
    }
}
