namespace Starter.Blazor.Core.Routes;

public static class ProjectEndpoints
{
    public static string GetDetails(string id) => $"Project/{id}";

    public static readonly string GetList = "Project/search";

    public static readonly string Add = "Project/Create";

    public static string Update(string projectId) => $"Project/{projectId}";

    public static string Delete(string id) => $"Project/{id}";
}
