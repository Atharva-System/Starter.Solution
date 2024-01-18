using System.Text.Json.Serialization;

namespace Starter.Blazor.Modules.Login.Model;

public record RefreshTokenRequest(string Token, string RefreshToken);
