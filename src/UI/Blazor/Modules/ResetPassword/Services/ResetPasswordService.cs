using Starter.Blazor.Modules.ForgotPassword.Models;
using Starter.Blazor.Modules.ResetPassword.Models;
using System.Net.Http.Json;

namespace Starter.Blazor.Modules.ResetPassword.Services;

public class ResetPasswordService
{
    private readonly HttpClient _httpClient;

    public ResetPasswordService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
   
    public async Task<string> ResetPasswordAsync(ResetPasswordDto resetPassword)
    {


        var response = await _httpClient.PostAsJsonAsync("api/Auth/resetPassword", resetPassword);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        return "Failed to reset password";
    }
}
