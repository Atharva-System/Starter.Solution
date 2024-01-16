using Starter.Application.Contracts.Persistence.Services;
using Starter.Application.Exceptions;
using Starter.Application.Features.Common;
using Starter.Application.UnitOfWork;
using Starter.Domain.Entities;

namespace Starter.Application.Features.Projects.Command;
public record DeleteProjectCommandRequest(Guid id) : IRequest<ApiResponse<string>>;

public class DeleteProjectCommandHandler(IQueryUnitOfWork query,
    ICommandUnitOfWork command, ITaskService taskService) : IRequestHandler<DeleteProjectCommandRequest, ApiResponse<string>>
{
    private readonly ICommandUnitOfWork _command = command;
    private readonly IQueryUnitOfWork _query = query;
    private readonly ITaskService _taskService = taskService;

    public async Task<ApiResponse<string>> Handle(DeleteProjectCommandRequest request, CancellationToken cancellationToken)
    {
        var project = await _query.QueryRepository<Project>().GetByIdAsync(request.id.ToString());
        _ = project ?? throw new NotFoundException("ProjectId ", request.id);

        //Check any task created for project
        var projectTask = await _taskService.IsTaskCreatedForProject(request.id);

        if (projectTask)
        {
            throw new Exception($"Cannot delete as Task is created for project.");
        }

        _command.CommandRepository<Project>().Remove(project);
        var result = await _command.SaveAsync(cancellationToken);

        var response = new ApiResponse<string>
        {
            Success = result > 0,
            StatusCode = result > 0 ? HttpStatusCodes.OK : HttpStatusCodes.BadRequest,
            Data = result.ToString(),
            Message = result > 0 ? $"Project {ConstantMessages.DeletedSuccessfully}" : $"{ConstantMessages.FailedToCreate} project."
        };

        return response;
    }
}
