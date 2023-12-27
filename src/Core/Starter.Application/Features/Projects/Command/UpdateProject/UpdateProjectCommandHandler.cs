﻿using Starter.Application.Features.Common;
using Starter.Application.UnitOfWork;
using Starter.Domain.Entities;

namespace Starter.Application.Features.Projects.Command.UpdateProject;
public class UpdateProjectCommandHandler(ICommandUnitOfWork command) : IRequestHandler<UpdateProjectCommand, ApiResponse<int>>
{
    private readonly ICommandUnitOfWork _commandUnitofWork = command;

    public async Task<ApiResponse<int>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var existingProject = await _commandUnitofWork.CommandRepository<Project>().GetByIdAsync(request.Id, cancellationToken);

        if (existingProject == null)
        {
            return new ApiResponse<int>
            {
                Success = false,
                StatusCode = HttpStatusCodes.NotFound,
                Message = $"Project with ID {request.Id} not found.",
            };
        }

        existingProject.ProjectName = request.ProjectName;
        existingProject.Description = request.Description;
        existingProject.StartDate = request.StartDate;
        existingProject.EndDate = request.EndDate;
        existingProject.EstimatedHours = request.EstimatedHours;

        var saveResult = await _commandUnitofWork.SaveAsync(cancellationToken);

        var response = new ApiResponse<int>
        {
            Success = saveResult > 0,
            StatusCode = saveResult > 0 ? HttpStatusCodes.OK : HttpStatusCodes.BadRequest,
            Data = saveResult,
            Message = saveResult > 0 ? $"Project {ConstantMessages.UpdatedSuccessfully}" : "FailedToUpdate project.",
        };

        return response;
    }
}
