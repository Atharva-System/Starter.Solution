using Starter.Blazor.Modules.ForgotPassword.Models;
using System.Net.Http.Json;

namespace Starter.Blazor.Modules.ForgotPassword.Services;

public class ForgotPasswordService
{
    private readonly HttpClient _httpClient;

    public ForgotPasswordService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<string> ForgotPasswordAsync(ForgotPasswordDto forgotPassword)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Auth/forgotPassword", forgotPassword);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        return "Failed to send reset link";
    }
   
}
