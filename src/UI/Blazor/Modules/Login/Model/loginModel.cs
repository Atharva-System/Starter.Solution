using System.ComponentModel.DataAnnotations;

namespace Starter.Blazor.Modules.Login.Model;

public class loginModel
{
    [Required(ErrorMessage="Email is required")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}
