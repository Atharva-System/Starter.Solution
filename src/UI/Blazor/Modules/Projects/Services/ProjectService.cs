using Starter.Blazor.Core.Response;
using Starter.Blazor.Core.Routes;
using Starter.Blazor.Core.Services.IServices;
using Starter.Blazor.Modules.Common;
using Starter.Blazor.Modules.Projects.Models;
using Starter.Blazor.Shared.Response;

namespace Starter.Blazor.Modules.Projects.Services;

public class ProjectService(IApiHandler api, INotificationService notificationService) : IProjectService
{
    private readonly IApiHandler _api = api;
    private readonly INotificationService _notificationService = notificationService;

    public async Task<ApiResponse<string>> AddProjectAsync(AddEditProject projectDto)
    {
        try
        {
            return await _api.Post<ApiResponse<string>, AddEditProject>(ProjectEndpoints.Add, projectDto);
        }
        catch(Exception ex)
        {
            var errorResponse = await _api.ConvertStringToResponse<ApiResponse<string>>(ex.Message);
            await _notificationService.Message(errorResponse.Message);
            errorResponse.Data = null;
            return errorResponse;// Return null or handle error case appropriately
        }
    }

    public async Task<ApiResponse<string>> EditProject(ProjectDto projectDto)
    {
        try
        {
            return await _api.Put<ApiResponse<string>, ProjectDto>(ProjectEndpoints.Update(projectDto.Id), projectDto);
        }
        catch (Exception ex)
        {
            var errorResponse = await _api.ConvertStringToResponse<ApiResponse<string>>(ex.Message);
            await _notificationService.Message(errorResponse.Message);
            return errorResponse;// Return null or handle error case appropriately
        }
    }

    public async Task<ApiResponse<ProjectDetailsDto>> GetProjectDetails(string id)
    {
        try
        {
            return await _api.Get<ApiResponse<ProjectDetailsDto>>(ProjectEndpoints.GetDetails(id));
        }
        catch (Exception ex)
        {
            var errorResponse = await _api.ConvertStringToResponse<ApiResponse<object>>(ex.Message);
            await _notificationService.Failure(errorResponse.Messages);
            return null;// Return null or handle error case appropriately
        }
    }

    public async Task<PagedDataResponse<List<ProjectDto>>> GetProjectlistsAsync(PaginationRequest param)
    {
        try
        {
            return await _api.Post<PagedDataResponse<List<ProjectDto>>, PaginationRequest>(ProjectEndpoints.GetList, param);
        }
        catch (Exception ex)
        {
            var errorResponse = await _api.ConvertStringToResponse<ApiResponse<object>>(ex.Message);
            await _notificationService.Failure(errorResponse.Messages);
            return null;// Return null or handle error case appropriately
        }
    }

    public async Task<ApiResponse<string>> DeleteProject(string id)
    {
        try
        {
            return await _api.Delete<ApiResponse<string>>(ProjectEndpoints.Delete(id));
        }
        catch (Exception ex)
        {
            var errorResponse = await _api.ConvertStringToResponse<ApiResponse<string>>(ex.Message);
            await _notificationService.Failure(errorResponse.Messages);
            errorResponse.Data = null;
            return errorResponse;// Return null or handle error case appropriately
        }
    }
}

