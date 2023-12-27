using Starter.Application.Exceptions;
using Starter.Application.Features.Common;
using Starter.Application.UnitOfWork;
using Starter.Domain.Entities;

namespace Starter.Application.Features.Projects.Command;
public record DeleteProjectCommandRequest(Guid id) : IRequest<ApiResponse<string>>;

public class DeleteProjectCommandHandler(IQueryUnitOfWork query,
    ICommandUnitOfWork command) : IRequestHandler<DeleteProjectCommandRequest, ApiResponse<string>>
{
    private readonly ICommandUnitOfWork _command = command;
    private readonly IQueryUnitOfWork _query = query;

    public async Task<ApiResponse<string>> Handle(DeleteProjectCommandRequest request, CancellationToken cancellationToken)
    {
        var project = await _query.QueryRepository<Project>().GetByIdAsync(request.id.ToString());
        _ = project ?? throw new NotFoundException("ProjectId ", request.id);

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
