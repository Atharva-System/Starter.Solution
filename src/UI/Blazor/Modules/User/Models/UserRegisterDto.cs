using System.ComponentModel.DataAnnotations;

namespace Starter.Blazor.Modules.User.Models;

public class UserRegisterDto
{
    public string UserId { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$",
    ErrorMessage = "Password must meet requirements")]
    public string Password { get; set; }
}
