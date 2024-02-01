using Starter.Application.Contracts.Responses;
using Starter.Application.Features.Tasks.Dto;
using Starter.Application.Models.Specification.Filters;

namespace Starter.Application.Features.Tasks.Query;

public sealed record GetTasksQuery : IRequest<IPagedDataResponse<TaskListDto>>
{
    public PaginationFilter PaginationFilter { get; set; } = default!;
}
