using System.ComponentModel.DataAnnotations;

namespace Starter.Blazor.Modules.User.Models;

public class UserRegisterDto
{
    public string UserId { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$",
    ErrorMessage = "Minimum length should be 6 characters.\r\nPassword requirements: At least one uppercase letter (A-Z), one lowercase letter (a-z), and one non-alphanumeric character.")]
    public string Password { get; set; }
}
