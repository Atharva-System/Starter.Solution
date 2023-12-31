﻿using Starter.Application.Exceptions;
using Starter.Application.Features.Common;
using Starter.Application.Features.Projects.Dtos;
using Starter.Application.UnitOfWork;
using Starter.Domain.Entities;

namespace Starter.Application.Features.Projects.Query.GetProjectDetails;
public record GetProjectDetailsQueryRequest(Guid id) : IRequest<ApiResponse<ProjectDto>>;
public class GetProjectDetailsQueryHandler(IQueryUnitOfWork query) : IRequestHandler<GetProjectDetailsQueryRequest, ApiResponse<ProjectDto>>
{

    private readonly IQueryUnitOfWork _query = query;

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
