using Starter.Blazor.Modules.ResetPassword.Models;

namespace Starter.Blazor.Modules.ResetPassword.Services;

public interface IResetPasswordService
{
    Task<string> ResetPasswordAsync(ResetPasswordDto resetPassword);
}
