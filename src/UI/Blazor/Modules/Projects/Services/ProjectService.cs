﻿using System.Net.Http.Json;
using Starter.Blazor.Core.Response;
using Starter.Blazor.Modules.Common;
using Starter.Blazor.Modules.Projects.Models;
using Starter.Blazor.Shared.Response;

namespace Starter.Blazor.Modules.Projects.Services;

public class ProjectService(HttpClient http) : IProjectService
{
    private readonly HttpClient _http = http;

    public async Task<ApiResponse<int>> AddProjectAsync(AddEditProject projectDto)
    {
        try
        {
            var result = await _http.PostAsJsonAsync("api/Project/Create", projectDto);

            var newResponse = await result.Content.ReadFromJsonAsync<ApiResponse<int>>();

            if (newResponse != null && newResponse.Success)
            {
                return newResponse;
            }
            else
            {
                return new ApiResponse<int>
                {
                    Success = false,
                    Message = newResponse.Message,
                };
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return new ApiResponse<int>
            {
                Success = false,
            };
        }
    }

    public async Task<ApiResponse<ProjectDetailsDto>> GetProjectDetails(string id)
    {
        return await _http.GetFromJsonAsync<ApiResponse<ProjectDetailsDto>>($"api/Project/{id}");
    }

    public async Task<List<ProjectDto>> GetProjectlistsAsync(PaginationRequest param)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/Project/search", param);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PagedDataResponse<List<ProjectDto>>>();
            return result?.Data ?? [];
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return [];
        }
    }
}
