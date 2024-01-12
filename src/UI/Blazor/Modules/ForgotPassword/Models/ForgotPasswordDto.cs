using System.ComponentModel.DataAnnotations;

namespace Starter.Blazor.Modules.ForgotPassword.Models;

public class ForgotPasswordDto
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; }
}
