namespace Starter.Blazor.Core.Services.IServices;

public interface IApiHandler
{
    Task<T> Get<T>(string url);
    Task<T> Post<T,U>(string url, U model);
    Task<T> Put<T,U>(string url, U model);
    Task<T> Delete<T>(string url);
    Task<T> ConvertStringToResponse<T>(String responseMessage);
    void RemoveToken();
}
