using Microsoft.AspNetCore.Mvc;
using Starter.Application.Contracts.Identity;
using Starter.Application.Models.Authentication;

namespace Starter.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(ILogger<AuthController> logger, IAuthService authService) : ControllerBase
{
    private readonly ILogger<AuthController> _logger = logger;
    private readonly IAuthService _authService = authService;

    [HttpPost("signin")]
    public async Task<ActionResult<AuthenticationResponse>> SignInAsync(AuthenticationRequest request)
    {
        return Ok(await _authService.AuthenticateAsync(request));
    }


    [HttpPost("register")]
    public async Task<ActionResult<RegistrationResponse>> RegisterAsync(RegistrationRequest request)
    {
        return Ok(await _authService.RegisterAsync(request));
    }

    [HttpPost("refreshToken")]
    public async Task<ActionResult<RefreshTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request)
    {
        return Ok(await _authService.RefreshTokenAsync(request));
    }
}
