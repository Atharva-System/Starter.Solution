using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Starter.API.Controllers.Base;
using Starter.Application.Contracts.Identity;
using Starter.Application.Features.Common;
using Starter.Application.Models.Authentication;
using Starter.InfraStructure.Cors;

namespace Starter.API.Controllers;

public class AuthController(ILogger<AuthController> logger, IAuthService authService, IConfiguration configuration) : BaseApiController
{
    private readonly ILogger<AuthController> _logger = logger;
    private readonly IAuthService _authService = authService;
    private readonly IConfiguration _configuration = configuration;

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

    [HttpPost("forgotPassword")]
    public async Task<ApiResponse<string>> ForgotPassword(ForgotPasswordRequest request)
    {
        await _authService.ForgotPasswordAsync(request, GetOriginFromRequest());
        return new ApiResponse<string>
        {
            Success = true,
            Data = "Password reset link sent successfully.",
            StatusCode = HttpStatusCodes.OK
        };
    }

    [HttpPost("resetPassword")]
    public async Task<ApiResponse<string>> ResetPassword(ResetPasswordRequest request)
    {
        await _authService.ResetPasswordAsync(request.Email, request.Token, request.NewPassword);
        return new ApiResponse<string>
        {
            Success = true,
            Data = "Password reset successful.",
            StatusCode = HttpStatusCodes.OK
        };
    }

    private string GetOriginFromRequest()
    {
        var corsSettings = _configuration.GetSection(nameof(CorsSettings)).Get<CorsSettings>();

        if (corsSettings != null && corsSettings.Angular is not null)
        {
            return corsSettings.Angular;
        }
        return $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}";
    }
}
