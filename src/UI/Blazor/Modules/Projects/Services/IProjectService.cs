using Starter.Blazor.Core.Response;
using Starter.Blazor.Modules.Common;
using Starter.Blazor.Modules.Projects.Models;
using Starter.Blazor.Shared.Response;

namespace Starter.Blazor.Modules.Projects.Services;

public interface IProjectService
{
    Task<PagedDataResponse<List<ProjectDto>>> GetProjectlistsAsync(PaginationRequest param);
    Task<ApiResponse<string>> AddProjectAsync(AddEditProject projectDto);
    Task<ApiResponse<ProjectDetailsDto>> GetProjectDetails(string id);
    Task<ApiResponse<string>> EditProject(ProjectDto projectDto);
    Task<ApiResponse<string>> DeleteProject(string id);
}
