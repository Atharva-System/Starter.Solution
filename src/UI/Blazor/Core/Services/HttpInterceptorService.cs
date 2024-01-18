using System.Net.Http.Headers;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Starter.Blazor.Core.Constants;
using Starter.Blazor.Core.Services.IServices;
using Starter.Blazor.Modules.Login.Services.IServices;
using Toolbelt.Blazor;

namespace Starter.Blazor.Core.Services;

public class HttpInterceptorService : IHttpInterceptorService
{
    private readonly HttpClientInterceptor _interceptor;
    private readonly IAuthService _authService;
    private readonly ILocalStorageService _localStorageService;

    public HttpInterceptorService(
        HttpClientInterceptor interceptor, 
        IAuthService authService,
        ILocalStorageService localStorageService)
    {
        _interceptor = interceptor;
        _authService = authService;
        _localStorageService = localStorageService;
    }

    public void RegisterEvent() => _interceptor.BeforeSendAsync += InterceptBeforeHttpAsync;

    public async Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e)
    {
        var absPath = e.Request.RequestUri.AbsolutePath;
        if(
            !absPath.Contains("signin") && 
            !absPath.Contains("refreshToken") && 
            !absPath.Contains("forgotPassword") && 
            !absPath.Contains("resetPassword"))
        {
            try
            {
                await _authService.TryRefreshToken();
                var savedToken = await this._localStorageService.GetItemAsync<String>(StorageConstants.Local.AuthToken);
                e.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);
                return;
            }
            catch (Exception ex)
            {
                _authService.Logout();
            }
        }
    }

    public void DisposeEvent() => _interceptor.BeforeSendAsync -= InterceptBeforeHttpAsync;
    public void RegisterAfterEvent() => _interceptor.AfterSendAsync += InterceptAfterHttpAsync;

    public async Task InterceptAfterHttpAsync(object sender, HttpClientInterceptorEventArgs e)
    {
        if(e.Response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            try
            {
                _authService.TryRefreshToken();
                
            }
            catch (Exception ex)
            {
                _authService.Logout();
            }
        }
    }
    public void DisposeAfterEvent() => _interceptor.AfterSendAsync -= InterceptAfterHttpAsync;
}
