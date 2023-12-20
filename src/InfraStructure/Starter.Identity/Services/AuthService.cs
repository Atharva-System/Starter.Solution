using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Starter.Application.Contracts.Identity;
using Starter.Application.Exceptions;
using Starter.Application.Models.Authentication;
using Starter.Identity.Models;

namespace Starter.Identity.Services;

public class AuthService(UserManager<ApplicationUser> userManager,
                        IOptions<JwtSettings> jwtSettings,
                        IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
                        SignInManager<ApplicationUser> signInManager,
                        IAuthorizationService authorizationService) : IAuthService
{
    private readonly IAuthorizationService _authorizationService = authorizationService;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory = userClaimsPrincipalFactory;

    #region Public Methods
    public async Task<string?> GetUserNameAsync(string userId)
    {
        var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

        return user.UserName;
    }

    public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        _ = user ?? throw new NotFoundException("User ", request.Email);

        var result = await _signInManager.PasswordSignInAsync(user?.UserName!, request.Password, false, lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            throw new Exception($"Please provide valid credentials");
        }

        JwtSecurityToken jwtSecurityToken = await GenerateToken(user!);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        user.RefreshToken = GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);
        await _userManager.UpdateAsync(user);

        AuthenticationResponse response = new AuthenticationResponse
        {
            Id = user?.Id!,
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            RefreshToken = user.RefreshToken,
            Email = user?.Email!,
            UserName = user?.UserName!
        };
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        return response;
    }

    public async Task<RegistrationResponse> RegisterAsync(RegistrationRequest request)
    {
        var existingEmail = await _userManager.FindByEmailAsync(request.Email);

        if (existingEmail != null)
        {
            throw new Exception($"Email '{request.Email}' already exists.");
        }

        var user = new ApplicationUser
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.Email,
            EmailConfirmed = true
        };

        if (existingEmail == null)
        {
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {

                await _userManager.AddToRoleAsync(user, Constants.IdentityRole.Administrator);

                return new RegistrationResponse() { UserId = user.Id };
            }
            else
            {
                throw new Exception($"{string.Join(",", result.Errors.Select(p => p.Description))}");
            }
        }
        else
        {
            throw new Exception($"Email {request.Email} already exists.");
        }
    }

    public async Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var userPrincipal = GetPrincipalFromExpiredToken(request.Token);
        string? userEmail = userPrincipal?.FindFirstValue(JwtRegisteredClaimNames.Email);

        if (string.IsNullOrEmpty(userEmail))
        {
            throw new AuthenticationException("Authentication Failed.");
        }

        var user = await _userManager.FindByEmailAsync(userEmail);

        _ = user ?? throw new NotFoundException("User ", userEmail);

        if (user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            throw new AuthenticationException("Invalid Refresh Token.");
        }

        JwtSecurityToken jwtSecurityToken = await GenerateToken(user!);

        if (string.IsNullOrEmpty(user.RefreshToken) || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);
            await _userManager.UpdateAsync(user);
        }

        return new RefreshTokenResponse(new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken), user.RefreshToken, user.RefreshTokenExpiryTime);
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user == null)
        {
            return false;
        }

        var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

        var result = await _authorizationService.AuthorizeAsync(principal, policyName);

        return result.Succeeded;
    }
    public async Task<ChangePasswordResponse> ChangePasswordAsync(string userId, string currentPassword, string newPassword, string confirmPassword)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            throw new NotFoundException("User", userId);
        }
        if (newPassword != confirmPassword)
        {
            return new ChangePasswordResponse
            {
                Success = false,
                Message = "New password does not match the confirmed password."
            };
        }

        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

        if (!result.Succeeded)
        {
            return new ChangePasswordResponse
            {
                Success = false,
                Message = $"Failed to change password: {string.Join(",", result.Errors.Select(p => p.Description))}"
            };
        }

        return new ChangePasswordResponse
        {
            Success = true,
            Message = "Password changed successfully"
        };
    }

#endregion

#region Private Methods


    private async Task<JwtSecurityToken> GenerateToken(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        var roleClaims = new List<Claim>();

        for (int i = 0; i < roles.Count; i++)
        {
            roleClaims.Add(new Claim("roles", roles[i]));
        }

        var claims = new[]
        {
                new Claim(JwtRegisteredClaimNames.Name, user.FirstName+" "+ user.LastName),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim("uid", user.Id)
            }
        .Union(userClaims)
        .Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
            signingCredentials: signingCredentials);
        return jwtSecurityToken;
    }

    private string GenerateRefreshToken()
    {
        var numbers = new byte[32];
        using RandomNumberGenerator randomNumber = RandomNumberGenerator.Create();
        randomNumber.GetBytes(numbers);
        return Convert.ToBase64String(numbers);
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = false
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new UnauthorizedAccessException("Invalid Token.");
        }

        return principal;
    }
    #endregion
}
