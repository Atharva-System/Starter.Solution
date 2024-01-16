using Microsoft.AspNetCore.SignalR;
using Starter.Application.Contracts.Caching;
using Starter.Application.Interfaces;
using Starter.InfraStructure.Caching;

namespace Starter.InfraStructure.Hub;
public class NotifyService(IHubContext<NotificationHub> notificationHubContext, ICacheService cache) : INotifyService
{
    private readonly IHubContext<NotificationHub> _notificationHubContext = notificationHubContext;
    private readonly ICacheService _cache = cache;

    public async Task SendToClientAsync(string method, INotifyParam param, string userId, CancellationToken cancellationToken)
    {
        var connectionId = await _cache.GetAsync<string>(CacheKeyConstants.NotifyUser(userId), cancellationToken);

        if (!string.IsNullOrEmpty(connectionId))
        {
            await _notificationHubContext.Clients.Client(connectionId).SendAsync(method, param, DateTime.Now);
        }
    }
}
