using Starter.Application.Contracts.Responses;

namespace Starter.Application.Features.Common;
public class ApiResponse<T> : IDataResponse<T>
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }

    public List<string> Messages { get; set; }
    public IDictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
}
