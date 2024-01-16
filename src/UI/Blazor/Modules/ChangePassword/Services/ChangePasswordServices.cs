using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Starter.Blazor.Core.Response;
using Starter.Blazor.Core.Services.IServices;
using Starter.Blazor.Modules.ChangePassword.Model;
using Starter.Blazor.Core.Routes;
using Starter.Blazor.Core.Constants;

namespace Starter.Blazor.Modules.ChangePassword.Services;

public class ChangePasswordServices : IChangePasswordServices
{
    private readonly IApiHandler _apiHandler;
    private readonly ILocalStorageService _localStorageService;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly INotificationService _notificationService;

    public ChangePasswordServices(
        IApiHandler apiHandler,
        ILocalStorageService localStorageService,
        AuthenticationStateProvider authenticationStateProvider,
        INotificationService notificationService,
        NavigationManager navManager)
    {
        _apiHandler = apiHandler;
        _localStorageService = localStorageService;
        _authenticationStateProvider = authenticationStateProvider;
        _notificationService = notificationService;
    }

    public async Task<ChangePasswordResponse> ChangePasswordAsync(ChangePasswordRequest chnagePassword)
    {
        try
        {
            var response = await _apiHandler.Post<ChangePasswordResponse, ChangePasswordRequest>(IdentityEndpoints.ChangePassword, chnagePassword);
            await _notificationService.Success(response.Message);
            return response;
        }
        catch (Exception ex)
        {
            var errorResponse = await _apiHandler.ConvertStringToResponse<ApiResponse<object>>(ex.Message);
            await _notificationService.Failure(errorResponse.Messages);
            return new ChangePasswordResponse { Message = String.Join(",", errorResponse.Messages), Success = errorResponse.Success };
        }
    }
}
