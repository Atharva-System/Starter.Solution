using Blazored.Modal;
using Blazored.Modal.Services;
using Starter.Blazor.Modules.Common;
using Starter.Blazor.Modules.Task.Model;

namespace Starter.Blazor.Modules.Task.Services;

public interface ITaskService : IModalService
{
    Task<string> CreateTaskAsync(TaskDetailsDto dto);
    Task<List<TaskListDto>> GetTasklistsAsync(PaginationRequest param);
}
