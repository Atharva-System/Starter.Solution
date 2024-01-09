using System.Net.Http.Json;
using Blazored.Modal.Services;
using Starter.Blazor.Core.Response;
using Starter.Blazor.Modules.Common;
using Starter.Blazor.Modules.Task.Model;
using Starter.Blazor.Shared.Response;

namespace Starter.Blazor.Modules.Task.Services;

public class TaskServices(HttpClient http) : ModalService, ITaskService
{
    private readonly HttpClient _httpClient = http;

    public event Func<Task<string>> OnClose;
    public List<ProjectListDto> Projects { get; set; } = new List<ProjectListDto>();

    public async Task<string> CreateTaskAsync(TaskDetailsDto dto)
    {
        try
        {
            var apiUrl = $"api/Task/Create";

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

    public async Task<List<ProjectListDto>> GetProjectlistsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/Task/projects");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<List<ProjectListDto>>();
            this.Projects = result;
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return [];
        }
    }

    public async Task<List<TaskListDto>> GetTasklistsAsync(PaginationRequest param)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/Task/search", param);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PagedDataResponse<List<TaskListDto>>>();
            return result?.Data ?? [];
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return [];
        }
    }
}
