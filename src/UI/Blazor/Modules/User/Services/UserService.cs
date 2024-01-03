﻿using System.Net.Http.Json;
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
                var user = await response.Content.ReadFromJsonAsync<UpdateProfileDto>();
                return user;
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
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

    public async Task<string> InviteUserAsync(InviteUserDto userDto)
    {
        try
        {
            var result = await _httpClient.PostAsJsonAsync("api/Users/invite-user", userDto);

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
}
