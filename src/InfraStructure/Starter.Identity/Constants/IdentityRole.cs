using System.Collections.ObjectModel;
using Starter.Domain.Constant;

namespace Starter.Identity.Constants;
public class IdentityRole
{
    public const string Administrator = nameof(Roles.Administrator);
    public const string SubAdmin = nameof(Roles.SubAdmin);
    public const string User = nameof(Roles.User);

    public static IReadOnlyList<string> DefaultRoles { get; } = new ReadOnlyCollection<string>(new[]
    {
        Administrator,
        SubAdmin,
        User
    });

    public static bool IsDefault(string roleName) => DefaultRoles.Any(r => r == roleName);
}
