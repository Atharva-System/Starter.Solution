using Starter.Blazor.Modules.User.Pages;

namespace Starter.Blazor.Core.Routes;

public static class UserEndpoints
{
    public static readonly string GetProfile = "api/Users/get-profile-details";

    public static string GetById(string id) => $"api/Users/{id}";

    public static string Update(string id) => $"api/Users/{id}";

    public static string GetAcceptInviteDetails(string id) => $"api/Users/get-invite-details/{id}";

    public static readonly string Search = "api/Users/search";

    public static readonly string AcceptInvite = "api/Users/accept-invite";

    public static readonly string InviteUser = "api/Users/invite-user";

    public static string UpdateUserProfile(string UserId) => $"api/Users/update-profile/{UserId}";

    public static string DeleteUser(string id) => $"api/Users/{id}";

}
