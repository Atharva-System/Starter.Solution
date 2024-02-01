namespace Starter.Blazor.Modules.Login.Model;

public class RefreshTokenResponse
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}
