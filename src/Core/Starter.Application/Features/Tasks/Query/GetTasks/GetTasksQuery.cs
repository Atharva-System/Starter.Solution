using MediatR;
using Starter.Application.Contracts.Responses;
using Starter.Application.Models.Task;
using Starter.Application.Models.Specification.Filters;
using System.Collections.Generic;
using Starter.Application.Features.Tasks.Dto;
using Starter.Application.Models.Task.Dto;

namespace Starter.Application.Features.Tasks.Query
{
    public class GetTasksQuery : IRequest<IPagedDataResponse<TaskListDto>>
    {
        public TaskFilter Filter { get; set; } = default!;
    }
}
