using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using Starter.Application.Contracts.Persistence.Repositoris.Queries;
using Starter.Application.Contracts.Responses;
using Starter.Application.Features.Common;
using Starter.Application.Features.Projects.Dtos;
using Starter.Persistence.Database;
using Starter.Persistence.Repositories.Base;
using Starter.Persistence.Services;

namespace Starter.Persistence.Repositories.Project.Query;
public class ProjectQueryRepository : QueryRepository<Starter.Domain.Entities.Project>, IProjectQueryRepository
{
    public ProjectQueryRepository(AppDbContext context) : base(context)
    { }
    public async Task<IPagedDataResponse<ProjectListDto>> SearchAsync(ISpecification<ProjectListDto> spec, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var projectList = from c in context.Projects.AsNoTracking()
                          select new ProjectListDto()
                          {
                              Description = c.Description,
                              EndDate = c.EndDate,
                              EstimatedHours = c.EstimatedHours,
                              ProjectName = c.ProjectName,
                              StartDate = c.StartDate
                          };

        var projects = await projectList.ApplySpecification(spec);

        var count = await projectList.ApplySpecificationCount(spec);

        return new PagedApiResponse<ProjectListDto>(projects, count, pageNumber, pageSize);
    }
}
