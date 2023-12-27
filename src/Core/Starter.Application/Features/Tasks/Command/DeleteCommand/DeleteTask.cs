using Starter.Application.Exceptions;
using Starter.Application.Features.Common;
using Starter.Application.UnitOfWork;

namespace Starter.Application.Features.Tasks.Command;
public record DeleteTaskCommandRequest(Guid id) : IRequest<ApiResponse<string>>;

public class DeleteTaskCommandHandler(IQueryUnitOfWork query,
    ICommandUnitOfWork command) : IRequestHandler<DeleteTaskCommandRequest, ApiResponse<string>>
{
    private readonly ICommandUnitOfWork _command = command;
    private readonly IQueryUnitOfWork _query = query;

    public async Task<ApiResponse<string>> Handle(DeleteTaskCommandRequest request, CancellationToken cancellationToken)
    {
        var task = await _query.QueryRepository<Domain.Entities.Tasks>().GetByIdAsync(request.id.ToString());
        _ = task ?? throw new NotFoundException("TaskId ", request.id);

        _command.CommandRepository<Domain.Entities.Tasks>().Remove(task);
        var result = await _command.SaveAsync(cancellationToken);

        var response = new ApiResponse<string>
        {
            Success = result > 0,
            StatusCode = result > 0 ? HttpStatusCodes.OK : HttpStatusCodes.BadRequest,
            Data = result.ToString(),
            Message = result > 0 ? $"Task {ConstantMessages.DeletedSuccessfully}" : $"{ConstantMessages.FailedToCreate} task."
        };

        return response;
    }
}
