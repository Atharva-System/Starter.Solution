using Ardalis.Specification;
using Starter.Application.Contracts.Responses;
using Starter.Application.Features.Tasks.Dto;
using Starter.Application.Models.Specification;
using Starter.Application.Models.Specification.Filters;
using Starter.Application.UnitOfWork;

namespace Starter.Application.Features.Tasks.Query;

public class GetTasksRequestSpec : EntitiesByPaginationFilterSpec<TaskListDto>
{
    public GetTasksRequestSpec(GetTasksQuery request)
        : base(request.PaginationFilter) =>
        Query.OrderByDescending(c => c.StartDate, !request.PaginationFilter.HasOrderBy());
}

public class GetTasksQueryHandler(IQueryUnitOfWork query) : IRequestHandler<GetTasksQuery, IPagedDataResponse<TaskListDto>>
{
    private readonly IQueryUnitOfWork _query = query;

    public async Task<IPagedDataResponse<TaskListDto>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetTasksRequestSpec(request);

        return await _query._taskQueryRepository.SearchAsync(spec, request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize, cancellationToken);
    }
}
