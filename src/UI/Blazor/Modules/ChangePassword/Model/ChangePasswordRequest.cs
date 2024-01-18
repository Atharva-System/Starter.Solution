using System.ComponentModel.DataAnnotations;

namespace Starter.Blazor.Modules.ChangePassword.Model;

public class ChangePasswordRequest
{
    [Required(ErrorMessage = "Current Password is required.")]
    public string CurrentPassword { get; set; }

    [Required(ErrorMessage = "New Password is required.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$",
    ErrorMessage = "Minimum length should be 6 characters.\r\nPassword requirements: At least one uppercase letter (A-Z), one lowercase letter (a-z), and one non-alphanumeric character.")]
    public string NewPassword { get; set; }

    [Required(ErrorMessage = "Confirm Password is required.")]
    [Compare("NewPassword", ErrorMessage = "Password do not match")]
    public string ConfirmPassword { get; set; }
}
