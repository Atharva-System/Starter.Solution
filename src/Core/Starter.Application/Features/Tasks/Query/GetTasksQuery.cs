using MediatR;
using Starter.Application.Contracts.Responses;
using Starter.Application.Models.Task;
using Starter.Application.Models.Specification.Filters;
using System.Collections.Generic;

namespace Starter.Application.Features.Tasks.Query
{
    public class GetTasksQuery : IRequest<IPagedDataResponse<TaskDto>>
    {
        public TaskFilter Filter { get; set; } = default!;
    }
}
