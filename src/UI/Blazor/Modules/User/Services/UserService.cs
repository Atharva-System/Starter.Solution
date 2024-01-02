using System.Net.Http.Headers;
using System.Net.Http.Json;
using Starter.Blazor.Modules.User.Models;
using Starter.Blazor.Shared.Response;

namespace Starter.Blazor.Modules.User.Services;

public class UserService
{
    private readonly HttpClient _httpClient;
    public UserService(HttpClient http)
    {
        _httpClient = http;
    }

    public async Task<List<UserlistDto>> GetUserlistsAsync()
    {
        try {
          //  _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiU3RhcnRlciBBZG1pbmlzdHJhdG9yIiwic3ViIjoibWVAc3RhcnRlci5jb20iLCJqdGkiOiIxMGEzNmQzZi1mMTg5LTRjNzItOWRhMC0xNzg3NmZhNjM1NTAiLCJlbWFpbCI6Im1lQHN0YXJ0ZXIuY29tIiwidWlkIjoiOWYzMTUyMWItN2U4Mi00ZWJhLWJkOTQtYjVjMzM1YzJkZTIwIiwicm9sZXMiOiJBZG1pbmlzdHJhdG9yIiwiZXhwIjoxNzA0MTgyMTI5LCJpc3MiOiJTdGFydGVySWRlbnRpdHkiLCJhdWQiOiJTdGFydGVySWRlbnRpdHlVc2VyIn0.sZsrwpy0ZmRtuVqCtq95jL2G5rIEIg2dGgTfq4x9R1k");
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
            return new List<UserlistDto>();
        }
    }
}
