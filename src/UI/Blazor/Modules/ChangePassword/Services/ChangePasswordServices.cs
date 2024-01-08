using System.Net.Http.Json;
using Starter.Blazor.Core.Response;
using Starter.Blazor.Modules.ChangePassword.Model;

namespace Starter.Blazor.Modules.ChangePassword.Services;

public class ChangePasswordServices(HttpClient http) : IChangePasswordServices
{
    private readonly HttpClient _httpClient = http;

    public async Task<ApiResponse<string>> ChangePasswordAsync(ChangePasswordRequest chnagePassword)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/changePassword", chnagePassword);
            var newResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();

            if (newResponse != null && newResponse.Success)
            {
                return newResponse;
            }
            else
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Messages = newResponse.Messages,
                };
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return new ApiResponse<string>
            {
                Success = false,
            };
        }
    }
}
