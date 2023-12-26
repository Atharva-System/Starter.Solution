using Starter.Application.Models.Authentication;
using System.Threading.Tasks;

namespace Starter.Application.Contracts.Identity;

public interface IAuthService : ITransientService
{
    Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
    Task<RegistrationResponse> RegisterAsync(RegistrationRequest request);
    Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request);
    Task<bool> IsInRoleAsync(string userId, string role);
    Task<bool> AuthorizeAsync(string userId, string policyName);
    Task<string?> GetUserNameAsync(string userId);
    Task<ChangePasswordResponse> ChangePasswordAsync(string userId, string currentPassword, string newPassword, string confirmPassword);
    Task ForgotPasswordAsync(ForgotPasswordRequest request);
    Task ResetPasswordAsync(string email, string token, string newPassword);

   // Task ResetPasswordAsync(string email, string token, string newPassword);
}
