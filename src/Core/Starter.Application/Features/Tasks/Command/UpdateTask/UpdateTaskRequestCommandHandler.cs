﻿using Starter.Application.Features.Common;
using Starter.Application.UnitOfWork;

namespace Starter.Application.Features.Tasks.Command.UpdateTask;
public class UpdateTaskRequestCommandHandler(ICommandUnitOfWork command) : IRequestHandler<UpdateTaskRequestCommand, ApiResponse<string>>
{
    private readonly ICommandUnitOfWork _commandUnitofWork = command;

    public async Task<ApiResponse<string>> Handle(UpdateTaskRequestCommand request, CancellationToken cancellationToken)
    {
        var existingTask = await _commandUnitofWork.CommandRepository<Starter.Domain.Entities.Tasks>().GetByIdAsync(request.Id, cancellationToken);

        if (existingTask == null)
        {
            return new ApiResponse<string>
            {
                Success = false,
                StatusCode = HttpStatusCodes.NotFound,
                Message = $"Task with ID {request.Id} not found.",
            };
        }

        existingTask.TaskName = request.TaskName;
        existingTask.Description = request.Description;
        existingTask.StartDate = request.StartDate;
        existingTask.EndDate = request.EndDate;
        existingTask.Status = request.Status;
        existingTask.Priority = request.Priority;
        existingTask.ProjectId = request.ProjectId;
        existingTask.AssignedTo = request.AssignedTo;

        await _commandUnitofWork.SaveAsync(cancellationToken);

        var response = new ApiResponse<string>
        {
            Success = true,
            StatusCode = HttpStatusCodes.OK,
            Data = "Task updated successfully",
            Message = $"Task {ConstantMessages.UpdatedSuccessfully}",
        };

        return response;
    }
}
