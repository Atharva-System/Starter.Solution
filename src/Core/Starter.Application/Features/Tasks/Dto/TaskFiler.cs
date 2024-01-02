using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Starter.Application.Features.Tasks.Dto;
using Starter.Application.Models.Specification;
using Starter.Application.Models.Specification.Filters;
using Starter.Application.Models.Users;

namespace Starter.Application.Models.Task.Dto;
public class TaskFilter : PaginationFilter
{
   
}

public class GetSearchTaskRequestSpec : EntitiesByPaginationFilterSpec<TaskListDto>
{
    public GetSearchTaskRequestSpec(TaskFilter request)
    : base(request) =>
        Query.OrderByDescending(c => c.Id, !request.HasOrderBy());
}

