using Starter.Blazor.Modules.User.Pages;

namespace Starter.Blazor.Core.Routes;

public static class UserEndpoints
{
    public static readonly string GetProfile = "Users/get-profile-details";

    public static string GetById(string id) => $"Users/{id}";

    public static string Update(string id) => $"Users/{id}";

    public static string GetAcceptInviteDetails(string id) => $"Users/get-invite-details/{id}";

    public static readonly string Search = "Users/search";

    public static readonly string AcceptInvite = "Users/accept-invite";

    public static readonly string InviteUser = "Users/invite-user";

    public static string UpdateUserProfile(string UserId) => $"Users/update-profile/{UserId}";

    public static string DeleteUser(string id) => $"Users/{id}";

}
