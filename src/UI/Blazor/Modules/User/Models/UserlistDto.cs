namespace Starter.Blazor.Modules.User.Models;

public class UserlistDto
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Status { get; set; }
    public string RoleId { get; set; }
    public string Role { get; set; }
    public string FullName { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
}
