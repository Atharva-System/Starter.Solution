using Ardalis.Specification;
using Starter.Application.Contracts.Persistence.Repositoris.Base;
using Starter.Application.Contracts.Responses;
using Starter.Application.Features.Common;
using Starter.Application.Features.Tasks.Dto;

namespace Starter.Application.Contracts.Persistence.Repositoris.Task;
public interface ITaskQueryRepository : IQueryRepository<Domain.Entities.Tasks>
{
    Task<IPagedDataResponse<TaskListDto>> SearchAsync(ISpecification<TaskListDto> spec, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<ApiResponse<List<ProjectDropdownDto>>> GetProjectListAsync();
}
