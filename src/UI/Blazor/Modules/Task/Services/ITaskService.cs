using Blazored.Modal;
using Blazored.Modal.Services;
using Starter.Blazor.Core.Response;
using Starter.Blazor.Modules.Common;
using Starter.Blazor.Modules.Task.Model;

namespace Starter.Blazor.Modules.Task.Services;

public interface ITaskService : IModalService
{
    Task<string> CreateTaskAsync(TaskListDto dto);
    Task<string> UpdateTaskAsync(Guid Id, TaskListDto dto);
    List<ProjectListDto> Projects { get; }
    Task<List<TaskListDto>> GetTasklistsAsync(PaginationRequest param);
    Task<ApiResponse<List<ProjectListDto>>> GetProjectlistsAsync();
    Task<ApiResponse<TaskListDto>> GetTaskDetails(Guid Id);
    Task<TaskListDto> DeleteTaskAsync(Guid Id);
    Task<ApiResponse<List<EnumTypeViewDto>>> GetStatuslistsAsync();
    Task<ApiResponse<List<EnumTypeViewDto>>> GetPrioritylistsAsync();
    Task<ApiResponse<List<TaskAssigneeDto>>> GetAssigneeListAsync();
}
