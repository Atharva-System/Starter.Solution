namespace Starter.Domain.Constant;

public static class Roles_old
{
    public const string Administrator = nameof(Administrator);
    public const string User = nameof(User);

}


public class RolesClaim
{

    public readonly List<string[]> roleClaims = new List<string[]>
        {
            new string[] { Roles_old.Administrator, "TodoItem", "Create" },
            new string[] { "User", "TodoItem", "Read" },
            // Add more role claims as needed
        };

}

