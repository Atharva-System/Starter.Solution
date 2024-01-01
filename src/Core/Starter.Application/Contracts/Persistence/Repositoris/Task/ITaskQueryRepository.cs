using Ardalis.Specification;
using Starter.Application.Contracts.Persistence.Repositoris.Base;
using Starter.Application.Contracts.Responses;
using Starter.Application.Models.Task;
using System.Threading;
using System.Threading.Tasks;

namespace Starter.Application.Contracts.Persistence.Repositoris.Task
{
    public interface ITaskQueryRepository : IQueryRepository<Domain.Entities.Tasks>
    {
        Task<IPagedDataResponse<TaskDto>> SearchAsync(ISpecification<TaskDto> spec, int pageNumber, int pageSize, CancellationToken cancellationToken);
    }
}
