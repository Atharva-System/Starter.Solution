using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.JSInterop;
using Starter.Blazor.Core.AuthProviders;
using Starter.Blazor.Core.Response;
using Starter.Blazor.Modules.Common;
using Starter.Blazor.Modules.User.Models;
using Starter.Blazor.Shared.Response;


namespace Starter.Blazor.Modules.User.Services;

public class UserService(HttpClient http, ILocalStorageService localStorageService, IJSRuntime jsRuntime, UserAuthID UserAuthId) : IUserService
{
    private readonly HttpClient _httpClient = http;
    private readonly ILocalStorageService _localStorageService = localStorageService;
    private readonly IJSRuntime _jsRuntime = jsRuntime;
    private readonly UserAuthID _UserAuthId = UserAuthId;

    public async Task<UpdateProfileDto> GetProfileDetailAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/Users/get-profile-details");
            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadFromJsonAsync<ApiResponse<UpdateProfileDto>>();
                return user.Data;
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }
    public async Task<UserlistDto> GetUserDetailsByIdAsync(string userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/Users/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var userDetails = await response.Content.ReadFromJsonAsync<ApiResponse<UserlistDto>>();


                if (userDetails != null && userDetails.Data != null)
                {
                    return userDetails.Data;
                }
                else
                {
                    // Handle the case where userDetails or userDetails.Data is null
                    Console.WriteLine("User details not available.");
                    return null; // or throw an exception, return a default value, etc.
                }
            }
        }
        catch (Exception ex)
        {
            // Handle or log the exception
            Console.WriteLine($"Error: {ex.Message}");
        }

        return null; // Return null or handle error case appropriately
    }






    public async Task<ApiResponse<string>> UpdateUserAsync(UserlistDto userDto)
    {
        try
        {
            var result = await _httpClient.PutAsJsonAsync($"api/Users/{userDto.Id}", userDto);

            var newResponse = await result.Content.ReadFromJsonAsync<ApiResponse<string>>();

            return newResponse;
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

    public async Task<string> UpdateUserProfileAsync(string UserId,UpdateProfileDto userDto)
    {
        try
        {
            var apiUrl = $"api/Users/{UserId}/update-profile";
            userDto.Id = UserAuthId.GetUserId();

            var result = await _httpClient.PutAsJsonAsync(apiUrl, userDto);

            result.EnsureSuccessStatusCode();

            var newResponse = await result.Content.ReadFromJsonAsync<ApiResponse<string>>();

            if (newResponse != null && newResponse.Success)
            {
                return newResponse.Data;
            }

            return "";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return "";
        }
    }

    public async Task<ApiResponse<string>> DeleteUser(UserlistDto user)
    {
            var result = await _httpClient.DeleteAsync($"api/Users/{user.Id}");

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
}
