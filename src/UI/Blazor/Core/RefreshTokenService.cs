﻿using Microsoft.AspNetCore.Components.Authorization;
using Starter.Blazor.Modules.Login.Services.IServices;

namespace Starter.Blazor.Core;

public class RefreshTokenService
{
    private readonly AuthenticationStateProvider _authProvider;
    private readonly IAuthService _authService;

    public RefreshTokenService(AuthenticationStateProvider authProvider, IAuthService authService)
    {
        _authProvider = authProvider;
        _authService = authService;
    }
    public async Task<string> TryRefreshToken()
    {
        var authState = await _authProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        var exp = user.FindFirst(c => c.Type.Equals("exp")).Value;
        var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));

        var timeUTC = DateTime.UtcNow;

        var diff = expTime - timeUTC;
        if (diff.TotalMinutes <= 2)
            _authService.RefreshToken();

        return string.Empty;
    }
}
