namespace Starter.Application.Models.Users;
public class UserInviteDto
{
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string InvitedBy { get; set; } = default!;
}
