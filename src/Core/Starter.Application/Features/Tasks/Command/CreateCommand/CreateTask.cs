using Starter.Application.Features.Common;
using Starter.Application.UnitOfWork;
using Starter.Domain.Events;

namespace Starter.Application.Features.Tasks.Command;
public sealed class CreateTaskCommandRequest : IRequest<ApiResponse<int>>
{
    public string? TaskName { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Status { get; set; }
    public int Priority { get; set; }
    public Guid ProjectId { get; set; }
    public string? AssignedTo { get; set; }
}

public class CreateTaskCommandHandler(ICommandUnitOfWork command) : IRequestHandler<CreateTaskCommandRequest, ApiResponse<int>>
{
    private readonly ICommandUnitOfWork _commandUnitofWork = command;

    public async Task<ApiResponse<int>> Handle(CreateTaskCommandRequest request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.Tasks
        {
            TaskName = request.TaskName,
            Description = request.Description,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Status = request.Status,
            Priority = request.Priority,
            ProjectId = request.ProjectId,
            AssignedTo = request.AssignedTo,
        };

        await _commandUnitofWork.CommandRepository<Domain.Entities.Tasks>().AddAsync(entity);
        entity.AddDomainEvent(new TaskCreatedDomainEvent(entity.AssignedTo!, entity.Id.ToString(), entity.TaskName!));
        var saveResult = await _commandUnitofWork.SaveAsync(cancellationToken);
        var response = new ApiResponse<int>
        {
            Success = saveResult > 0,
            StatusCode = saveResult > 0 ? HttpStatusCodes.Created : HttpStatusCodes.BadRequest,
            Data = saveResult,
            Message = saveResult > 0 ? $"Task {ConstantMessages.AddedSuccesfully}" : $"{ConstantMessages.FailedToCreate} task."
        };
        return response;
    }
}
