namespace Starter.Application.Interfaces;
public interface INotifyService : ITransientService
{
    Task SendToClientAsync(string method, INotifyParam param, string userId, CancellationToken cancellationToken);
}
