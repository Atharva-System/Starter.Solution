namespace Starter.Blazor.Modules.Dashboard.Services;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Starter.Blazor.Modules.Projects.Models;

public class ProjectService
{
    private readonly HttpClient _httpClient;

    public ProjectService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Guid> CreateProject(ProjectDto project)
    {
        var response = await _httpClient.PostAsJsonAsync("https://localhost:7195/api/Project/Create", project);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Guid>();
    }

    public async Task<List<ProjectDto>> GetProjects()
    {
        return await _httpClient.GetFromJsonAsync<List<ProjectDto>>("https://localhost:7195/api/project/Search");
    }

    public async Task<ProjectDto> GetProjectDetails(Guid projectId)
    {
        return await _httpClient.GetFromJsonAsync<ProjectDto>($"api/project/{projectId}");
    }

    public async Task DeleteProject(Guid projectId)
    {
        var response = await _httpClient.DeleteAsync($"api/project/{projectId}");
        response.EnsureSuccessStatusCode();
    }
}
