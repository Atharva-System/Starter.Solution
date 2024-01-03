using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Starter.Blazor.Core.Response;
using Starter.Blazor.Modules.Login.Model;

namespace Starter.Blazor.Modules.Login.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _http;
    private readonly AuthenticationStateProvider _authStateProvider;
    private const string AuthBaseURL = "api/auth/";
    private readonly ILocalStorageService _localStorage;
    private readonly NavigationManager _navigationManager;
    public AuthService(HttpClient http, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage, NavigationManager navigationManager)
    {
        _http = http;
        _authStateProvider = authStateProvider;
        _localStorage = localStorage;
        _navigationManager = navigationManager;
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

    public async Task<string> RefreshToken()
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");
        var request = new RefreshTokenRequest() { CurrentToken = token };

        var result = await _http.PostAsJsonAsync($"{AuthBaseURL}refreshToken", request);
        var response = await result.Content.ReadFromJsonAsync<ApiResponse<AuthResponseDto>>();

        if (response != null)
        {
            if (response.Success)
            {
                await _localStorage.SetItemAsync("authToken", response.Data.Token);
                await _authStateProvider.GetAuthenticationStateAsync();
                return response.Data.Token;
            }
            else
            {
                _navigationManager.NavigateTo("/login");
            }
        }

        return string.Empty;
    }
}
