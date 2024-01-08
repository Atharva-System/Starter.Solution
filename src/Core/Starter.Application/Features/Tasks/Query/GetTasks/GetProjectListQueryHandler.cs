using Starter.Application.Features.Tasks.Dto;
using Starter.Application.UnitOfWork;

namespace Starter.Application.Features.Tasks.Query.GetTasks;
public class GetProjectListQueryHandler : IRequestHandler<GetProjectListQuery,List<ProjectDropdownDto>>
{
    private readonly IQueryUnitOfWork _query;

    public GetProjectListQueryHandler(IQueryUnitOfWork query)
    {
        _query = query;
    }

    public async Task<List<ProjectDropdownDto>> Handle(GetProjectListQuery request, CancellationToken cancellationToken)
    {
        var response = await _query._taskQueryRepository.GetProjectListAsync(request.userId);

        return response;
    }
}
