namespace Starter.Application.Models.Authentication;

public record RefreshTokenRequest(string Token, string RefreshToken);
