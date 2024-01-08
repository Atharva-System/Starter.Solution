namespace Starter.Application.Models.Users;
public class UserListDto
{
    public string Id { get; set; } = default!;

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string Status { get; set; } = default!;

    public string RoleId { get; set; } = default!;

    public string Role { get; set; } = default!;

    public string FullName { get; set; } = default!;

    public DateTimeOffset? CreatedOn { get; set; }
}
