using System.ComponentModel.DataAnnotations;

namespace Starter.Blazor.Modules.Login.Model;

public class loginModel
{
    [Required(ErrorMessage="Email is required")]
    [EmailAddress(ErrorMessage ="Enter Proper email address")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Password is required")]
    [MinLength(6,ErrorMessage ="Password should be minimum 6 characters long")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$",ErrorMessage =@"Password required atleast one uppercase letter (A-Z) one lowercase letter (a-z), and one non-alphanumeric character")]
    public string Password { get; set; }
}
