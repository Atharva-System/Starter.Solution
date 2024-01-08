namespace Starter.Application.Contracts.Responses;
public interface IDataResponse<T> : IResponse
{
    T Data { get; }
    List<string> Messages { get; }
}
