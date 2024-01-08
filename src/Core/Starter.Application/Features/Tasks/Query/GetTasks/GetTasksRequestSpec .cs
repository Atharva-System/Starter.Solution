using Ardalis.Specification;
using Starter.Application.Features.Tasks.Dto;
using Starter.Application.Models.Task;
using Starter.Application.Models.Task.Dto;

namespace Starter.Application.Features.Tasks.Query
{
    public class GetTasksRequestSpec : Specification<TaskListDto>
    {
        public GetTasksRequestSpec(TaskFilter filter)
        {
            Query.OrderBy(t => t.StartDate)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize);
        }
    }
}
