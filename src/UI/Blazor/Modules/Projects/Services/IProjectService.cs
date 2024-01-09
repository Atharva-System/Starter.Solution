using Starter.Blazor.Core.Response;
using Starter.Blazor.Modules.Common;
using Starter.Blazor.Modules.Projects.Models;

namespace Starter.Blazor.Modules.Projects.Services;

public interface IProjectService
{
    Task<List<ProjectDto>> GetProjectlistsAsync(PaginationRequest param);
    Task<ApiResponse<int>> AddProjectAsync(AddEditProject projectDto);
    Task<ApiResponse<ProjectDetailsDto>> GetProjectDetails(string id);
    Task<ApiResponse<string>> EditProject(string id, AddEditProject editProject);
    Task<ApiResponse<string>> DeleteProject(ProjectDto user);
}
