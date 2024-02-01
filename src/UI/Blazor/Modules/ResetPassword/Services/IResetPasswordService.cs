using Starter.Blazor.Core.Response;
using Starter.Blazor.Modules.ResetPassword.Models;

namespace Starter.Blazor.Modules.ResetPassword.Services;

public interface IResetPasswordService
{
    Task<ApiResponse<string>> ResetPasswordAsync(ResetPasswordDto resetPassword);
}
