﻿using Starter.Application.Features.Common;
using Starter.Application.UnitOfWork;
using Starter.Domain.Entities;

namespace Starter.Application.Features.Projects.Command.CreateCommand;

public sealed record CreateProjectCommandRequest : IRequest<ApiResponse<string>>
{
    public string? ProjectName { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal EstimatedHours { get; set; }
}

public class CreateProjectCommandHandler(ICommandUnitOfWork command) : IRequestHandler<CreateProjectCommandRequest, ApiResponse<string>>
{
    private readonly ICommandUnitOfWork _commandUnitofWork = command;

    public async Task<ApiResponse<string>> Handle(CreateProjectCommandRequest request, CancellationToken cancellationToken)
    {
        var entity = new Project
        {
            ProjectName = request.ProjectName,
            Description = request.Description,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            EstimatedHours = request.EstimatedHours,
        };

        await _commandUnitofWork.CommandRepository<Project>().AddAsync(entity);
        var saveResult = await _commandUnitofWork.SaveAsync(cancellationToken);
        var response = new ApiResponse<string>
        {
            Success = saveResult > 0,
            StatusCode = saveResult > 0 ? HttpStatusCodes.Created : HttpStatusCodes.BadRequest,
            Data = "Project created successfully!",
            Message = saveResult > 0 ? $"Project {ConstantMessages.AddedSuccesfully}" : $"{ConstantMessages.FailedToCreate} project."
        };
        return response;
    }
}
