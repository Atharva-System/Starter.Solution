using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Starter.Application.Contracts.Persistence.Repositoris.Base;
using Starter.Application.Contracts.Responses;
using Starter.Application.Features.Tasks.Dto;

namespace Starter.Application.Contracts.Persistence.Repositoris.Task;
public interface ITaskQueryRepository : IQueryRepository<Domain.Entities.Tasks>
{
    Task<IPagedDataResponse<TaskListDto>> SearchAsync(ISpecification<TaskListDto> spec, int pageNumber, int pageSize, CancellationToken cancellationToken);
}
