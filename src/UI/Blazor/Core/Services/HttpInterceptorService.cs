using Microsoft.AspNetCore.Components;
using Starter.Blazor.Core.Services.IServices;
using Starter.Blazor.Modules.Login.Services.IServices;
using Toolbelt.Blazor;

namespace Starter.Blazor.Core.Services;

public class HttpInterceptorService : IHttpInterceptorService
{
    private readonly HttpClientInterceptor _interceptor;
    private readonly NavigationManager _navManager;
    private readonly INotificationService _notificationService;
    private readonly IAuthService _authService;

    public HttpInterceptorService(
        HttpClientInterceptor interceptor, 
        NavigationManager navManager, 
        INotificationService notificationService,
        IAuthService authService)
    {
        _interceptor = interceptor;
        _navManager = navManager;
        _notificationService = notificationService;
        _authService = authService;
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
                _authService.TryRefreshToken();
                
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
