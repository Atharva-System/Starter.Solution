namespace Starter.Blazor.Modules.ResetPassword.Models;

public class ResetPasswordDto
{
    public string Email { get; set; }
    public string Token { get; set; }
    public string NewPassword { get; set; }
}
