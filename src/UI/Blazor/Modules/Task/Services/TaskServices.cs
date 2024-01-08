using System.Net.Http.Json;
using Blazored.Modal.Services;
using Starter.Blazor.Core.Response;
using Starter.Blazor.Modules.Task.Model;

namespace Starter.Blazor.Modules.Task.Services;

public class TaskServices(HttpClient http) : ModalService, ITaskService
{
    private readonly HttpClient _httpClient = http;

    public event Func<Task<string>> OnClose;

    public async Task<string> CancelAsync()
    {
        throw new NotImplementedException();
    }

    public Task<string> CloseAsync(ModalResult result = null)
    {
        throw new NotImplementedException();
    }

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
}
