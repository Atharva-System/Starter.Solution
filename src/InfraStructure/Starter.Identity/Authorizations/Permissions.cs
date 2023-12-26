using System.Collections.ObjectModel;

namespace Starter.Identity.Authorizations;
public class AllPermissions
{
    private static readonly Permission[] _all =
   [
       new("General", Action.View, Resource.General, IsBasic: true),
       new("View Dashboard", Action.View, Resource.AdminDashboard, IsAdmin: true),
       new("View Todo", Action.View, Resource.Todo, IsAdmin: true, IsBasic: true),
       new("Search Todo", Action.Search, Resource.Todo, IsAdmin: true),
       new("Create Todo", Action.Create, Resource.Todo, IsAdmin: true),
       new("Update Todo", Action.Update, Resource.Todo, IsAdmin: true),
       new("Delete Todo", Action.Delete, Resource.Todo, IsAdmin: true),
       new("Export Todo", Action.Export, Resource.Todo, IsAdmin: true),
       new("View Users", Action.View, Resource.Users, IsAdmin: true),
       new("Search Users", Action.Search, Resource.Users, IsAdmin: true),
       new("Update Users", Action.Update, Resource.Users, IsAdmin: true),
       new("Delete Users", Action.Delete, Resource.Users, IsAdmin: true),
       new("invite-user", Action.Create, Resource.Users, IsAdmin: true),
       new("Create Project", Action.Create, Resource.Project, IsAdmin: true),
       new("Create Task", Action.Create, Resource.Task, IsAdmin: true),
       new("View Task", Action.View, Resource.Task, IsAdmin: true),
       new("Search Project", Action.Search, Resource.Project, IsAdmin: true),
   ];

    public static IReadOnlyList<Permission> All { get; } = new ReadOnlyCollection<Permission>(_all);
    public static IReadOnlyList<Permission> Root { get; } = new ReadOnlyCollection<Permission>(_all.Where(p => p.IsRoot || p.IsAdmin || p.IsBasic).ToArray());
    public static IReadOnlyList<Permission> Admin { get; } = new ReadOnlyCollection<Permission>(_all.Where(p => !p.IsRoot).ToArray());
    public static IReadOnlyList<Permission> User { get; } = new ReadOnlyCollection<Permission>(_all.Where(p => p.IsAdmin || p.IsBasic).ToArray());

}

public record Permission(string Description, string Action, string Resource, bool IsAdmin = false, bool IsBasic = false, bool IsRoot = false)
{
    public string Name => NameFor(Action, Resource);
    public static string NameFor(string action, string resource) => $"Permissions.{resource}.{action}";
}
