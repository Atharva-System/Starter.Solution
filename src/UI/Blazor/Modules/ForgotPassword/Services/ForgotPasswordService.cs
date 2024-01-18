using Blazored.LocalStorage;
using Starter.Blazor.Core.Response;
using Starter.Blazor.Core.Routes;
using Starter.Blazor.Core.Services.IServices;
using Starter.Blazor.Modules.ForgotPassword.Models;
using Starter.Blazor.Modules.User.Models;
using Starter.Blazor.Modules.User.Pages;
using Starter.Blazor.Shared.Constants;
using System.Net.Http.Json;

namespace Starter.Blazor.Modules.ForgotPassword.Services;

public class ForgotPasswordService : IForgotPasswordService
{
    private readonly IApiHandler _api;
    private readonly ILocalStorageService _localStorageService;
    private readonly INotificationService _notificationService;
    public ForgotPasswordService(IApiHandler api, ILocalStorageService localStorageService, INotificationService notificationService)
    {
        _api = api;
        _localStorageService = localStorageService;
        _notificationService = notificationService;
    }

    public async Task<ApiResponse<string>> ForgotPasswordAsync(ForgotPasswordDto forgotPassword)
    {
        try
        {
            return await _api.Post<ApiResponse<string>, ForgotPasswordDto>(IdentityEndpoints.ForgotPassword, forgotPassword);
            // var response = await _httpClient.PostAsJsonAsync("Auth/forgotPassword", forgotPassword);

        }
        catch (Exception ex)
        {
            var errorResponse = await _api.ConvertStringToResponse<ApiResponse<string>>(ex.Message);
            await _notificationService.Failure(errorResponse.Messages);
            errorResponse.Data = null;
            return errorResponse;
        }
    }
}

