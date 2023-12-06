namespace Starter.Identity.Authorization;
public class RolesClaim
{
    public readonly List<string[]> RoleClaims = new List<string[]>
    {
        new string[] { Roles.Administrator, Resources.TodoItem, Permissions.Create },
        new string[] { Roles.Administrator, Resources.TodoItem, Permissions.Read },
        new string[] { Roles.User, Resources.TodoItem, Permissions.Read },
        // Add more role claims as needed
    };
}
