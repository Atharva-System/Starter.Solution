using Microsoft.AspNetCore.Components;
using Starter.Blazor.Modules.Login.Services;
using Toolbelt.Blazor;

namespace Starter.Blazor.Core;

public class HttpInterceptorService
{
    private readonly HttpClientInterceptor _interceptor;
    private readonly NavigationManager _navManager;
    private readonly IAuthService _authService;
    public HttpInterceptorService(HttpClientInterceptor interceptor, NavigationManager navManager, IAuthService authService)
    {
        _interceptor = interceptor;
        _navManager = navManager;
        _authService = authService;
    }
}
