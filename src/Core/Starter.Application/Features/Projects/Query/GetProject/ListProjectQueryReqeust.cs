using Starter.Application.Contracts.Responses;
using Starter.Application.Features.Projects.Dtos;
using Starter.Application.Models.Specification.Filters;

namespace Starter.Application.Features.Projects.Query.GetProject;
public sealed record ListProjectQueryReqeust : IRequest<IPagedDataResponse<ProjectListDto>>
{
    public PaginationFilter PaginationFilter { get; set; } = default!;
}
