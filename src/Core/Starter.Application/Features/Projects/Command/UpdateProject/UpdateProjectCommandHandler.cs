﻿using Starter.Application.Features.Common;
using Starter.Application.UnitOfWork;
using Starter.Domain.Entities;

namespace Starter.Application.Features.Projects.Command.UpdateProject;
public class UpdateProjectCommandHandler(ICommandUnitOfWork command) : IRequestHandler<UpdateProjectCommand, ApiResponse<string>>
{
    private readonly ICommandUnitOfWork _commandUnitofWork = command;

    public async Task<ApiResponse<string>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var existingProject = await _commandUnitofWork.CommandRepository<Project>().GetByIdAsync(request.Id, cancellationToken);

        if (existingProject == null)
        {
            return new ApiResponse<string>
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

        await _commandUnitofWork.SaveAsync(cancellationToken);

        var response = new ApiResponse<string>
        {
            Success = true,
            StatusCode =  HttpStatusCodes.OK ,
            Data = "Project update successfully",
            Message = $"Project {ConstantMessages.UpdatedSuccessfully}",
        };

        return response;
    }
}
