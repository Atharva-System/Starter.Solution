using System.ComponentModel.DataAnnotations;

namespace Starter.Blazor.Modules.ChangePassword.Model;

public class ChangePasswordRequest
{
    [Required(ErrorMessage = "Current Password is required.")]
    public string CurrentPassword { get; set; }

    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$",
        ErrorMessage = "Password must meet requirements")]
    public string NewPassword { get; set; }

    [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }
}
