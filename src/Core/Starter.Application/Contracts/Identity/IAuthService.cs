﻿using Starter.Application.Models.Authentication;

namespace Starter.Application.Contracts.Identity;

public interface IAuthService
{
    Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
    Task<RegistrationResponse> RegisterAsync(RegistrationRequest request);

    Task<bool> IsInRoleAsync(string userId, string role);
    Task<bool> AuthorizeAsync(string userId, string policyName);
    Task<string?> GetUserNameAsync(string userId);
}
