using System.ComponentModel.DataAnnotations;

namespace Starter.Blazor.Modules.User.Models;

public class UpdateProfileDto
{
    public string Id { get; set; }

    [Required(ErrorMessage = "First Name is required.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; }
    public string? ImageUrl { get; set; }
}
