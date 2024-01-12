using Starter.Blazor.Core.Response;
using Starter.Blazor.Modules.Login.Model;

namespace Starter.Blazor.Modules.Login.Services.IServices;

public interface IAuthService
{
    Task<ApiResponse<AuthResponseDto>> Login(loginModel request);
    void RefreshToken();
    void TryRefreshToken();
    void Logout();
}
