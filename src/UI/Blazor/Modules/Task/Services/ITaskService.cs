using Blazored.Modal;
using Blazored.Modal.Services;
using Starter.Blazor.Core.Response;
using Starter.Blazor.Modules.Common;
using Starter.Blazor.Modules.Task.Model;

namespace Starter.Blazor.Modules.Task.Services;

public interface ITaskService : IModalService
{
    Task<string> CreateTaskAsync(TaskDetailsDto dto);
    List<ProjectListDto> Projects { get; }
    Task<List<TaskListDto>> GetTasklistsAsync(PaginationRequest param);
    Task<List<ProjectListDto>> GetProjectlistsAsync();
    Task<TaskListDto> GetTaskDetails(Guid Id);
    Task<TaskListDto> DeleteTaskAsync(Guid Id);
    Task<ApiResponse<List<EnumTypeViewDto>>> GetStatuslistsAsync();
    Task<ApiResponse<List<EnumTypeViewDto>>> GetPrioritylistsAsync();
}
