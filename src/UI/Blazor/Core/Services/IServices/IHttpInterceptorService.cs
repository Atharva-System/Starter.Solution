using Toolbelt.Blazor;

namespace Starter.Blazor.Core.Services.IServices;

public interface IHttpInterceptorService
{
    void RegisterEvent();

    Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e);

    void DisposeEvent();
}
