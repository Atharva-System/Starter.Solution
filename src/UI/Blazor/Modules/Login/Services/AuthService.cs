using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Starter.Blazor.Core.Authentication;
using Starter.Blazor.Core.Constants;
using Starter.Blazor.Core.Response;
using Starter.Blazor.Core.Routes;
using Starter.Blazor.Core.Services.IServices;
using Starter.Blazor.Modules.Login.Model;
using Starter.Blazor.Modules.Login.Services.IServices;

namespace Starter.Blazor.Modules.Login.Services;

public class AuthService : IAuthService
{
    private readonly IApiHandler _apiHandler;
    private readonly ILocalStorageService _localStorageService;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly INotificationService _notificationService;
    private readonly NavigationManager _navManager;

    public AuthService(
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
        _navManager = navManager;
    }

    public async Task<ApiResponse<AuthResponseDto>> Login(loginModel model)
    {
        try
        {
            var response = await _apiHandler.Post<ApiResponse<AuthResponseDto>, loginModel>(TokenEndpoints.Get, model);
            if (response.StatusCode == HttpStatusCodes.OK && response.Success == true)
            {
                await _notificationService.Success(response.Message);
                await _localStorageService.SetItemAsync(StorageConstants.Local.AuthToken, response.Data.Token);
                await _localStorageService.SetItemAsync(StorageConstants.Local.RefreshToken, response.Data.RefreshToken);
                await _localStorageService.SetItemAsync(StorageConstants.Local.Id, response.Data.Id);
                await _localStorageService.SetItemAsStringAsync(StorageConstants.Local.Username, response.Data.UserName);
                await _localStorageService.SetItemAsStringAsync(StorageConstants.Local.Email, response.Data.Email);

                await ((AppStateProvider)this._authenticationStateProvider).StateChangeAsync();

                _navManager.NavigateTo("/");
            }
            else
            {
                await _notificationService.Failure(response.Messages);
            }
            return response;
        }

        catch (Exception ex)
        {
            var errorResponse = await _apiHandler.ConvertStringToResponse<ApiResponse<object>>(ex.Message);
            await _notificationService.Failure(errorResponse.Messages);
            return new ApiResponse<AuthResponseDto> { Message = String.Join(",", errorResponse.Messages), Success = errorResponse.Success };
        }
    }

    public async void Logout()
    {
        await _localStorageService.RemoveItemAsync(StorageConstants.Local.AuthToken);
        await _localStorageService.RemoveItemAsync(StorageConstants.Local.RefreshToken);
        await _localStorageService.RemoveItemAsync(StorageConstants.Local.Id);
        await _localStorageService.RemoveItemAsync(StorageConstants.Local.Username);
        await _localStorageService.RemoveItemAsync(StorageConstants.Local.Email);
        await ((AppStateProvider) _authenticationStateProvider).MarkAsLoggedOut();
        this._apiHandler.RemoveToken();
        _navManager.NavigateTo("/");
    }

    public async System.Threading.Tasks.Task RefreshToken()
    {
        var request = new RefreshTokenRequest(await _localStorageService.GetItemAsync<string>(StorageConstants.Local.AuthToken),
            await _localStorageService.GetItemAsync<string>(StorageConstants.Local.RefreshToken));
        var response = await this._apiHandler.Post<RefreshTokenResponse,RefreshTokenRequest>(TokenEndpoints.Refresh, request);
        if (!string.IsNullOrEmpty(response.Token))
        {
            await _localStorageService.SetItemAsync<string>(StorageConstants.Local.AuthToken, response.Token);
            await _localStorageService.SetItemAsync<string>(StorageConstants.Local.RefreshToken, response.RefreshToken);

            await ((AppStateProvider)this._authenticationStateProvider).StateChangeAsync();
        }
        else
        {
            await _notificationService.Failure(new List<string> { "Token failed" });
            _navManager.NavigateTo("/");
        }
    }

    public async System.Threading.Tasks.Task TryRefreshToken()
    {
        try
        {
            var availableToken = await _localStorageService.GetItemAsync<string>(StorageConstants.Local.RefreshToken);
            if (string.IsNullOrEmpty(availableToken)) _navManager.NavigateTo("/");
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var exp = user.FindFirst(c => c.Type.Equals("exp"))?.Value;
            var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
            var timeUTC = DateTime.UtcNow;
            var diff = expTime - timeUTC;

            if (diff.TotalMinutes <= 1)
            {
                await RefreshToken();
                return;
            }
        }
        catch (Exception ex)
        {
            throw ex;
            return;
        }
    }
}
