using System.Net.Http.Json;
using Starter.Blazor.Core.Response;
using Starter.Blazor.Modules.Common;
using Starter.Blazor.Modules.User.Models;
using Starter.Blazor.Shared.Response;

namespace Starter.Blazor.Modules.User.Services;

public class UserService(HttpClient http) : IUserService
{
    private readonly HttpClient _httpClient = http;

    public async Task<ApiResponse<AcceptInviteDto>> GetAcceptInviteDetails(string userId)
    {
        return await _httpClient.GetFromJsonAsync<ApiResponse<AcceptInviteDto>>($"api/Users/get-invite-details/{userId}");
    }

    public async Task<List<UserlistDto>> GetUserlistsAsync(PaginationRequest param)
    {
        try {
            var response = await _httpClient.PostAsJsonAsync("api/Users/search", param);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PagedDataResponse<List<UserlistDto>>>();

            return result?.Data ?? [];
        }
        catch (Exception ex)
        {
            // Log or handle the exception
            Console.WriteLine($"Error: {ex.Message}");
            return [];
        }
    }

    public async Task<ApiResponse<string>> AcceptInvite(UserRegisterDto userRegister)
    {
        try
        {
            var result = await _httpClient.PostAsJsonAsync("api/Users/accept-invite", userRegister);
            var newResponse = await result.Content.ReadFromJsonAsync<ApiResponse<string>>();
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

    public async Task<ApiResponse<string>> InviteUserAsync(InviteUserDto userDto)
    {
        try
        {
            var result = await _httpClient.PostAsJsonAsync("api/Users/invite-user", userDto);

            var newResponse = await result.Content.ReadFromJsonAsync<ApiResponse<string>>();

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
