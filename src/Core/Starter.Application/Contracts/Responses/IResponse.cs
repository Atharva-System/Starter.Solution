namespace Starter.Application.Contracts.Responses;
public interface IResponse
{
    bool Success { get; }
    int StatusCode { get; }
    string Message { get; }
}
