using Starter.Application.Features.Common;
using Starter.Application.Features.Tasks.Dto;

namespace Starter.Application.Contracts.Persistence.Services;
public interface IUsersPersistenceService : ITransientService
{
    Task<ApiResponse<List<TaskAssigneeDto>>> GetAssigneeForTaskAsync();
}
