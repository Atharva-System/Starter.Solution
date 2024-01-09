using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using Starter.Application.Contracts.Persistence.Repositoris.Task;
using Starter.Application.Contracts.Responses;
using Starter.Application.Extensions;
using Starter.Application.Features.Common;
using Starter.Application.Features.Tasks.Dto;
using Starter.Domain.Enums;
using Starter.Persistence.Database;
using Starter.Persistence.Repositories.Base;
using Starter.Persistence.Services;


namespace Starter.Persistence.Repositories.Tasks.Query;

public class TaskQueryRepository : QueryRepository<Starter.Domain.Entities.Tasks>, ITaskQueryRepository
{
    public TaskQueryRepository(AppDbContext context) : base(context)
    { }

    public async Task<List<ProjectDropdownDto>> GetProjectListAsync(string UserId)
    {
        var projectList = await context.Projects.AsNoTracking()
                          .Select(p => new ProjectDropdownDto
                          {
                              Id = p.Id,
                              ProjectName = p.ProjectName
                          }).ToListAsync();

        return projectList;
    }

    public async Task<IPagedDataResponse<TaskListDto>> SearchAsync(ISpecification<TaskListDto> spec, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var taskList = from t in context.Tasks.AsNoTracking()
                           select new TaskListDto
                           {
                               Id = t.Id,
                               TaskName = t.TaskName,
                               Description = t.Description,
                               StartDate = t.StartDate,
                               EndDate = t.EndDate,
                               Status = t.Status.ToString(),
                               Priority = t.Priority.ToString(),
                               ProjectId = t.ProjectId,
                               AssignedTo = t.AssignedTo,
                               CreatedOn = t.CreatedOn.Date,
                               CreatedBy = t.CreatedBy,
                               ModifiedOn = t.ModifiedOn.DateTime,
                               ModifiedBy = t.ModifiedBy
                           };

        var tasks = taskList.ApplySpecification(spec);

        var count = taskList.ApplySpecificationToListCount(spec);

        return new PagedApiResponse<TaskListDto>(count, pageNumber, pageSize) { Data = tasks };
    }
}

