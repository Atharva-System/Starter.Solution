using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Starter.Application.Models.Specification;
using Starter.Application.Models.Specification.Filters;
using Starter.Application.Models.Users;

namespace Starter.Application.Models.Task;
public class TaskFilter : PaginationFilter
{
   
}

public class GetSearchTaskRequestSpec : EntitiesByPaginationFilterSpec<TaskDto>
{
    public GetSearchTaskRequestSpec(TaskFilter request)
    : base(request) =>
        Query.OrderByDescending(c => c.Id, !request.HasOrderBy());
}

