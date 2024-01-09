using System.ComponentModel.DataAnnotations;

namespace Starter.Blazor.Modules.User.Models;

public class InviteUserDto
{
    [Required(ErrorMessage = "First Name is required")]
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; }

    public string Id { get; set; }
}
