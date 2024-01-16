using Microsoft.AspNetCore.SignalR;
using Starter.Application.Contracts.Caching;
using Starter.Application.Features.Common;
using Starter.InfraStructure.Caching;

namespace Starter.InfraStructure.Hub;
public class NotificationHub(ICacheService cache) : Microsoft.AspNetCore.SignalR.Hub
{
    private readonly ICacheService _cache = cache;
    public override async Task OnConnectedAsync()
    {
        var userId = GetUserIdFromAccessToken(Context);
        var connectionId = Context.ConnectionId;

        await _cache.SetAsync(CacheKeyConstants.NotifyUser(userId), connectionId);
        await Clients.Client(connectionId!)
        .SendAsync(NotifyMethodConstants.ReceiveMessage, "You are connected from SignalR", DateTime.Now);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exp)
    {
        var userId = GetUserIdFromAccessToken(Context);
        var connectionId = Context.ConnectionId;

        var storedConnectionId = await _cache.GetAsync<string>(CacheKeyConstants.NotifyUser(userId));

        if (connectionId == storedConnectionId)
        {
            await _cache.RemoveAsync(userId.ToString());
        }

        await Clients.Client(storedConnectionId!)
         .SendAsync(NotifyMethodConstants.ReceiveMessage, "You are disconnected from SignalR", DateTime.Now);
        await base.OnDisconnectedAsync(exp);
    }

    private string GetUserIdFromAccessToken(HubCallerContext context)
    {
        return context.User?.FindFirst("uid")?.Value ?? string.Empty;
    }
}

