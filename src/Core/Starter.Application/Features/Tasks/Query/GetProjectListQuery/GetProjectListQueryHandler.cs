using Starter.Application.Features.Common;
using Starter.Application.Features.Tasks.Dto;
using Starter.Application.UnitOfWork;

namespace Starter.Application.Features.Tasks.Query;
public class GetProjectListQueryHandler : IRequestHandler<GetProjectListQuery, ApiResponse<List<ProjectDropdownDto>>>
{
    private readonly IQueryUnitOfWork _query;

    public GetProjectListQueryHandler(IQueryUnitOfWork query)
    {
        _query = query;
    }

    public async Task<ApiResponse<List<ProjectDropdownDto>>> Handle(GetProjectListQuery request, CancellationToken cancellationToken)
    {
        return await _query._taskQueryRepository.GetProjectListAsync();
    }
}
