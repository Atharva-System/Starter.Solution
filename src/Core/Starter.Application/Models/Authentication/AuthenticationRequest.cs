namespace Starter.Application.Models.Authentication;

public class AuthenticationRequest
{
    public string Email { get; set; } = "me@starter.com";
    public string Password { get; set; } = "Starter1!";
}