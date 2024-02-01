using System.Net.Http.Json;
using Blazored.Modal.Services;
using Starter.Blazor.Core.Response;
using Starter.Blazor.Modules.Common;
using Starter.Blazor.Modules.Projects.Models;
using Starter.Blazor.Modules.Task.Model;
using Starter.Blazor.Shared.Response;
using static System.Net.WebRequestMethods;

namespace Starter.Blazor.Modules.Task.Services;

public class TaskServices(HttpClient http) : ModalService, ITaskService
{
    private readonly HttpClient _httpClient = http;

    public event Func<Task<string>> OnClose;
    public List<ProjectListDto> Projects { get; set; } = new List<ProjectListDto>();

    public async Task<string> CreateTaskAsync(TaskListDto dto)
    {
        try
        {
            var apiUrl = $"Task/Create";

            var result = await _httpClient.PostAsJsonAsync(apiUrl, dto);

            result.EnsureSuccessStatusCode();

            var newResponse = await result.Content.ReadFromJsonAsync<ApiResponse<string>>();

            if (newResponse != null && newResponse.Success)
            {
                return newResponse.Data;
            }

            return "";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return "";
        }
    }

    public async Task<ApiResponse<List<ProjectListDto>>> GetProjectlistsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("Task/projects");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<ProjectListDto>>>();
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }

    public async Task<PagedDataResponse<List<TaskListDto>>> GetTasklistsAsync(PaginationRequest param)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("Task/search", param);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<PagedDataResponse<List<TaskListDto>>>();
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }

    public async Task<ApiResponse<TaskListDto>> GetTaskDetails(string Id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"Task/{Guid.Parse(Id)}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync <ApiResponse<TaskListDto>>();
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }

    public async Task<ApiResponse<string>> DeleteTaskAsync(string Id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"Task/{Guid.Parse(Id)}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }

    public async Task<ApiResponse<List<EnumTypeViewDto>>> GetStatuslistsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("Task/status-list");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<EnumTypeViewDto>>>();
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }

    public async Task<ApiResponse<List<EnumTypeViewDto>>> GetPrioritylistsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("Task/priority-list");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<EnumTypeViewDto>>>();
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }

    public async Task<ApiResponse<List<TaskAssigneeDto>>> GetAssigneeListAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("Task/assignee-list");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<TaskAssigneeDto>>>();
            return result;
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }

    public async Task<string> UpdateTaskAsync(Guid Id, TaskListDto dto)
    {
        try
        {
            var apiUrl = $"Task/{Id}";

            var result = await _httpClient.PutAsJsonAsync(apiUrl, dto);

            result.EnsureSuccessStatusCode();

            var newResponse = await result.Content.ReadFromJsonAsync<ApiResponse<string>>();

            if (newResponse != null && newResponse.Success)
            {
                return newResponse.Data;
            }

            return "";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return "";
        }
    }
}
