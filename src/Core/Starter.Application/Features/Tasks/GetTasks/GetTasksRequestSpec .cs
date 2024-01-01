using Ardalis.Specification;
using Starter.Application.Models.Task;

namespace Starter.Application.Features.Tasks.GetTasks
{
    public class GetTasksRequestSpec : Specification<TaskDto>
    {
        public GetTasksRequestSpec(TaskFilter filter)
        {
            Query.OrderBy(t => t.StartDate)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize);
        }
    }
}
