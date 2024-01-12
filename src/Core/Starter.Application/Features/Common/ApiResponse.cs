using Starter.Application.Contracts.Responses;

namespace Starter.Application.Features.Common;
public class ApiResponse<T> : Response,IDataResponse<T>
{
    public T Data { get; set; }

    public List<string>? Messages { get; set; }
    public IDictionary<string, object>? Metadata { get; set; } = new Dictionary<string, object>();
}
