using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using Starter.Blazor.Core.Response;

namespace Starter.Blazor.Core.Extensions;

public static class ResponseExtensions
{
    internal static async Task<ApiResponse<T>> ToResponse<T>(this HttpResponseMessage response)
    {
        var responseAsString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<ApiResponse<T>>(responseAsString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.Preserve
        });
        return responseObject;
    }
}
