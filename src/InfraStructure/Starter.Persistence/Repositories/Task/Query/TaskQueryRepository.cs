using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using Starter.Application.Contracts.Persistence.Repositoris.Task;
using Starter.Application.Contracts.Persistence.Repositoris.TodoRepository;
using Starter.Application.Contracts.Responses;
using Starter.Application.Features.Common;
using Starter.Application.Models.Task;
using Starter.Persistence.Database;
using Starter.Persistence.Repositories.Base;
using Starter.Persistence.Services;


namespace Starter.Persistence.Repositories.Tasks.Query
{
    public class TaskQueryRepository : QueryRepository<Starter.Domain.Entities.Tasks>, ITaskQueryRepository
    {
        public TaskQueryRepository(AppDbContext context) : base(context)
        { }

        public async Task<IPagedDataResponse<TaskDto>> SearchAsync(ISpecification<TaskDto> spec, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var taskList = from t in context.Tasks.AsNoTracking()
                           select new TaskDto
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

            var tasks = await taskList.ApplySpecification(spec);
            
            var count = await taskList.ApplySpecificationCount(spec);

            return new PagedApiResponse<TaskDto>(count, pageNumber, pageSize) { Data = tasks };
        }
    }
}
