using System.Net.Http.Json;
using Starter.Blazor.Modules.User.Models;
using Starter.Blazor.Shared.Response;

namespace Starter.Blazor.Modules.User.Services;

public class UserService(HttpClient http)
{
    private readonly HttpClient _httpClient = http;

    public async Task<List<UserlistDto>> GetUserlistsAsync()
    {
        try {
            var response = await _httpClient.PostAsJsonAsync("/api/Users/search", new
            {
                pageNumber = 0,
                pageSize = 10,
                orderBy = new[] { "FullName" }
            });

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
}
