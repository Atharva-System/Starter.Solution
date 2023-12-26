using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Starter.API.Controllers.Base;
using Starter.Application.Contracts.Identity;
using Starter.Application.Models.Authentication;

namespace Starter.API.Controllers;

public class AuthController(ILogger<AuthController> logger, IAuthService authService) : BaseApiController
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
    [Authorize]
    [HttpPost("changePassword")]
    public async Task<ActionResult<ChangePasswordResponse>> ChangePasswordAsync(ChangePasswordRequest request)
    {
        var userId = User.FindFirstValue("uid");
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        try
        {
            var response = await _authService.ChangePasswordAsync(userId, request.CurrentPassword, request.NewPassword, request.ConfirmPassword);

            if (response.Success)
            {
                return Ok(response); 
            }
            else
            {
                return BadRequest(response); 
            }
        }
        catch (Exception ex)
        {
            var errorResponse = new ChangePasswordResponse
            {
                Success = false,
                Message = ex.Message
            };

            return BadRequest(errorResponse); 
        }
    }


}
