namespace Starter.Blazor.Core.Services.IServices;

public interface INotificationService
{
    Task Success(string Message);
    Task Failure(string Message);
    Task Message(string Message);
}
