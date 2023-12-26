using Ardalis.Specification;
using Starter.Application.Contracts.Responses;
using Starter.Application.Features.Projects.Dtos;
using Starter.Application.Models.Specification;
using Starter.Application.Models.Specification.Filters;
using Starter.Application.UnitOfWork;

namespace Starter.Application.Features.Projects.Query.GetProject;

public class GetProjectRequestSpec : EntitiesByPaginationFilterSpec<ProjectListDto>
{
    public GetProjectRequestSpec(ListProjectQueryReqeust request)
        : base(request.PaginationFilter) =>
        Query.OrderByDescending(c => c.StartDate, !request.PaginationFilter.HasOrderBy());
}

public class ListProjectQueryReqeustHandler(IQueryUnitOfWork query) : IRequestHandler<ListProjectQueryReqeust, IPagedDataResponse<ProjectListDto>>
{
    private readonly IQueryUnitOfWork _query = query;

    public async Task<IPagedDataResponse<ProjectListDto>> Handle(ListProjectQueryReqeust request, CancellationToken cancellationToken)
    {
        var spec = new GetProjectRequestSpec(request);

        return await _query.ProjectQuery.SearchAsync(spec, request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize, cancellationToken);
    }
}
