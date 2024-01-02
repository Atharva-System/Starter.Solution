using Starter.Blazor.Core.Response;
using Starter.Blazor.Modules.Login.Model;

namespace Starter.Blazor.Modules.Login.Services;

public interface IAuthService
{
    Task<AuthResponseDto> Login(loginModel request);
    Task<bool> IsUserAuthenticated();
}
