namespace Starter.Blazor.Core.AuthProviders;

public class UserAuthID
{
    private string userId;

    public string GetUserId()
    {
        return userId;
    }

    public void SetUserId(string newUserId)
    {
        userId = newUserId;
    }

    public void ClearUserId()
    {
        userId = null;
    }
}
