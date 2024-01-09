using System.Net.Http;
using System.Net.Http.Json;
using Starter.Blazor.Core.Response;
using Starter.Blazor.Modules.Common;
using Starter.Blazor.Modules.Projects.Models;
using Starter.Blazor.Shared.Response;

namespace Starter.Blazor.Modules.Projects.Services;

public class ProjectService(HttpClient http) : IProjectService
{
    private readonly HttpClient _http = http;

    public async Task<ApiResponse<string>> AddProjectAsync(AddEditProject projectDto)
    {
        try
        {
            var result = await _http.PostAsJsonAsync("api/Project/Create", projectDto);

            var newResponse = await result.Content.ReadFromJsonAsync<ApiResponse<string>>();

            if (newResponse != null && newResponse.Success)
            {
                return newResponse;
            }
            else
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = newResponse.Message,
                };
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return new ApiResponse<string>
            {
                Success = false,
            };
        }
    }

    public async Task<ApiResponse<string>> EditProject(ProjectDto projectDto)
    {
        var result = await _http.PutAsJsonAsync($"api/Project/{projectDto.Id}", projectDto);

        var response = await result.Content.ReadFromJsonAsync<ApiResponse<string>>();
        return response!;
    }

    public async Task<ApiResponse<ProjectDetailsDto>> GetProjectDetails(string id)
    {
        return await _http.GetFromJsonAsync<ApiResponse<ProjectDetailsDto>>($"api/Project/{id}");
    }

    public async Task<PagedDataResponse<List<ProjectDto>>> GetProjectlistsAsync(PaginationRequest param)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/Project/search", param);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PagedDataResponse<List<ProjectDto>>>();
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }
    public async Task<ApiResponse<string>> DeleteProject(ProjectDto project)
    {
        var result = await _http.DeleteAsync($"api/Project/{project.Id}");

        var newResponse = await result.Content.ReadFromJsonAsync<ApiResponse<string>>();

        if (newResponse != null && newResponse.Success)
        {
            return newResponse;
        }
        else
        {
            return new ApiResponse<string>
            {
                Success = false,
                Messages = newResponse.Messages,
            };
        }
    }

    public async Task<ApiResponse<string>> DeleteProject(string id)
    {
        var result = await _http.DeleteAsync($"api/Project/{id}");

        var newResponse = await result.Content.ReadFromJsonAsync<ApiResponse<string>>();

        if (newResponse != null && newResponse.Success)
        {
            return newResponse;
        }
        else
        {
            return new ApiResponse<string>
            {
                Success = false,
                Messages = newResponse.Messages,
            };
        }
    }
}

