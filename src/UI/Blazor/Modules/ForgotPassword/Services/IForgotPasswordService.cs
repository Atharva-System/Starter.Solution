using Starter.Blazor.Modules.ForgotPassword.Models;

namespace Starter.Blazor.Modules.ForgotPassword.Services;

public interface IForgotPasswordService
{
    Task<string> ForgotPasswordAsync(ForgotPasswordDto forgotPassword);
}
