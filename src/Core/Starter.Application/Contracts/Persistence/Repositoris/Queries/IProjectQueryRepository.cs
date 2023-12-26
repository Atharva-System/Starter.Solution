using Ardalis.Specification;
using Starter.Application.Contracts.Responses;
using Starter.Application.Features.Projects.Dtos;
using Starter.Domain.Entities;
using Starter.Application.Contracts.Persistence.Repositoris.Base;

namespace Starter.Application.Contracts.Persistence.Repositoris.Queries;
public interface IProjectQueryRepository : IQueryRepository<Project>
{
    Task<IPagedDataResponse<ProjectListDto>> SearchAsync(ISpecification<ProjectListDto> spec,
                                                            int pageNumber,
                                                            int pageSize,
                                                            CancellationToken cancellationToken);
}
