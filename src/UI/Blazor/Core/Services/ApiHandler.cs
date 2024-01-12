using System.Net.Http.Json;
using Starter.Blazor.Core.Services.IServices;
using Starter.Blazor.Core.Extensions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Starter.Blazor.Core.Services;

public class ApiHandler : IApiHandler
{
    private readonly HttpClient _httpClient;
    public ApiHandler(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<T> Delete<T>(string url)
    {
        try
        {
            var response = await _httpClient.DeleteAsync(url);
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest ||
                   response.StatusCode == System.Net.HttpStatusCode.NotFound ||
                   response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                   response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
            return await response.Content.ReadFromJsonAsync<T>();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<T> Get<T>(string url)
    {
        try
        {
            var response = await _httpClient.GetAsync(url);
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest ||
                   response.StatusCode == System.Net.HttpStatusCode.NotFound ||
                   response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                   response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }

            return await response.Content.ReadFromJsonAsync<T>();
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }

    public async Task<T> Post<T, U>(string url, U model)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync<U>(url, model);
            if(response.StatusCode == System.Net.HttpStatusCode.BadRequest ||
               response.StatusCode == System.Net.HttpStatusCode.NotFound ||
               response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
               response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                throw new  Exception(await response.Content.ReadAsStringAsync());
            }
            return await this.ConvertResponse<T>(response);
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    public async Task<T> Put<T, U>(string url, U model)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync<U>(url, model);
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest ||
               response.StatusCode == System.Net.HttpStatusCode.NotFound ||
               response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
               response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
            return await this.ConvertResponse<T>(response);
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    private async Task<T> ConvertResponse<T>(HttpResponseMessage response)
    {
        var responseAsString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<T>(responseAsString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.Preserve
        });
        return responseObject;
    }

    public async Task<T> ConvertStringToResponse<T>(String responseMessage)
    {
        var responseObject = JsonSerializer.Deserialize<T>(responseMessage, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.Preserve
        });
        return responseObject;
    }

    public void RemoveToken()
    {
        this._httpClient.DefaultRequestHeaders.Authorization = null;
    }
}
