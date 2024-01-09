using Starter.Application.Contracts.Persistence.Services;
using Starter.Application.Features.Common;
using Starter.Application.Features.Tasks.Dto;

namespace Starter.Application.Features.Tasks.Query;
public record GetTaskAssigneesQueryRequest : IRequest<ApiResponse<List<TaskAssigneeDto>>>;

public class GetTaskAssigneesQueryHandler(IUsersPersistenceService usersPersistenceService) : IRequestHandler<GetTaskAssigneesQueryRequest, ApiResponse<List<TaskAssigneeDto>>>
{
    private readonly IUsersPersistenceService _usersPersistenceService = usersPersistenceService;
    public async Task<ApiResponse<List<TaskAssigneeDto>>> Handle(GetTaskAssigneesQueryRequest request, CancellationToken cancellationToken)
    {
        return await _usersPersistenceService.GetAssigneeForTaskAsync();
    }
}
