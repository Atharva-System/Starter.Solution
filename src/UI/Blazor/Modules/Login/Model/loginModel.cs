using System.ComponentModel.DataAnnotations;

namespace Starter.Blazor.Modules.Login.Model;

public class loginModel
{
    [Required(ErrorMessage = "Please enter Email Address.")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Please enter Password.")]
    public string Password { get; set; }
}
