using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Starter.Blazor.Modules.Login.Model;

namespace Starter.Blazor.Modules.Login.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _http;
    private readonly AuthenticationStateProvider _authStateProvider;
    private const string AuthBaseURL = "api/auth/";
    public AuthService(HttpClient http, AuthenticationStateProvider authStateProvider)
    {
        _http = http;
        _authStateProvider = authStateProvider;
    }
    public async Task<bool> IsUserAuthenticated()
    {
        return (await _authStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
    }

    public async Task<AuthResponseDto> Login(loginModel request)
    {
        var result = await _http.PostAsJsonAsync<loginModel>($"{AuthBaseURL}signin", request);

        var content = await result.Content.ReadAsStringAsync();
        Console.WriteLine($"Raw Response Content: {content}");

        return await result.Content.ReadFromJsonAsync<AuthResponseDto>();
    }
}
