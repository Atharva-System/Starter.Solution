namespace Starter.Blazor.Core.Response;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T Data { get; set; }

    public int StatusCode { get; set; }
    public string Message { get; set; }
    public List<string> Messages { get; set; } = new List<string>();
}
