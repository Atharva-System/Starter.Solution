using System.Text.Json.Serialization;

namespace Starter.Blazor.Modules.Login.Model;

public class RefreshTokenRequest
{
    [JsonIgnore]
    public string? RefreshToken { get; set; } = string.Empty;

    public string CurrentToken { get; set; } = string.Empty;
}
