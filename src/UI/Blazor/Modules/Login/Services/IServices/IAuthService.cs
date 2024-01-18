using Starter.Blazor.Core.Response;
using Starter.Blazor.Modules.Login.Model;

namespace Starter.Blazor.Modules.Login.Services.IServices;

public interface IAuthService
{
    Task<ApiResponse<AuthResponseDto>> Login(loginModel request);
    System.Threading.Tasks.Task RefreshToken();
    System.Threading.Tasks.Task TryRefreshToken();
    void Logout();
}
