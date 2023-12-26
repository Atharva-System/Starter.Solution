using Starter.Application.Contracts.Identity;
using Starter.Application.Exceptions;
using Starter.Application.Features.Common;
using Starter.Application.Features.Tasks.Dto;
using Starter.Application.UnitOfWork;
using Starter.Domain.Enums;

namespace Starter.Application.Features.Tasks.Query;
public record GetTaskDetailsQueryRequest(Guid id) : IRequest<ApiResponse<TaskDetailsDto>>;

public class GetTaskDetailsQueryHandler(IQueryUnitOfWork query,
    IUsersService usersService) : IRequestHandler<GetTaskDetailsQueryRequest, ApiResponse<TaskDetailsDto>>
{
    private readonly IQueryUnitOfWork _query = query;
    private readonly IUsersService _usersService = usersService;
    public async Task<ApiResponse<TaskDetailsDto>> Handle(GetTaskDetailsQueryRequest request, CancellationToken cancellationToken)
    {
        var task = await _query.QueryRepository<Domain.Entities.Tasks>().GetByIdAsync(request.id.ToString());
        _ = task ?? throw new NotFoundException("TaskId ", request.id);

        var userDetails = await _usersService.GetUserDetailsForTaskAsync(task.AssignedTo!, cancellationToken);

        var taskDetailDto = new TaskDetailsDto
        {
            Id = task.Id,
            TaskName = task.TaskName,
            Description = task.Description,
            StartDate = task.StartDate,
            EndDate = task.EndDate,
            Status = task.Status,
            Priority = task.Priority,
            StatusName = ((TasksStatus)task.Status).ToString(),
            PriorityName = ((TaskPriority)task.Priority).ToString() ,
            ProjectId = task.ProjectId,
            AssignedTo = task.AssignedTo,
            AssignedToName = userDetails.FullName,
        };

        var response = new ApiResponse<TaskDetailsDto>
        {
            Success = taskDetailDto != null,
            StatusCode = taskDetailDto != null ? HttpStatusCodes.OK : HttpStatusCodes.BadRequest,
            Data = taskDetailDto!,
            Message = taskDetailDto != null ? $"Task {ConstantMessages.DataFound}" : $"{ConstantMessages.NotFound} task."
        };

        return response;
    }
}
