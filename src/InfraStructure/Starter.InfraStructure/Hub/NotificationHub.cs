using Microsoft.AspNetCore.SignalR;
using Starter.Application.Contracts.Caching;
using Starter.Application.Features.Common;
using Starter.Application.Models.Users;
using Starter.InfraStructure.Caching;

namespace Starter.InfraStructure.Hub;
public class NotificationHub(ICacheService cache) : Microsoft.AspNetCore.SignalR.Hub
{
    private readonly ICacheService _cache = cache;

    public override async Task OnConnectedAsync()
    {
        var userId = GetUserIdFromAccessToken(Context);
        var connectionId = Context.ConnectionId;
        SetUsersForChat(Context);
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
        RemoveUserFromChat(connectionId);
        await Clients.Client(storedConnectionId!)
         .SendAsync(NotifyMethodConstants.ReceiveMessage, "You are disconnected from SignalR", DateTime.Now);
        await base.OnDisconnectedAsync(exp);
    }

    private string GetUserIdFromAccessToken(HubCallerContext context)
    {
        return context.User?.FindFirst("uid")?.Value ?? string.Empty;
    }

    private async void SetUsersForChat(HubCallerContext context)
    {
        var users = (await _cache.GetAsync<List<ChatUser>>(CacheKeyConstants.ChatUsers)) ?? new List<ChatUser>();

        users.Add(new ChatUser
        {
            ConnectionId = Context.ConnectionId,
            Name = context.User?.FindFirst("name")?.Value ?? string.Empty,
            UserId = context.User?.FindFirst("uid")?.Value ?? string.Empty,
            Date = DateTime.Now,
            Active = true,
        });

        await UpdateChatUsersCache(users);
    }

    private async void RemoveUserFromChat(string connectionId)
    {
        var users = (await _cache.GetAsync<List<ChatUser>>(CacheKeyConstants.ChatUsers)) ?? new List<ChatUser>();

        var userToRemove = users.FirstOrDefault(u => u.ConnectionId == connectionId);
        if (userToRemove != null)
        {
            users.Remove(userToRemove);
            await UpdateChatUsersCache(users);
        }
    }

    private async Task UpdateChatUsersCache(List<ChatUser> users)
    {
        await _cache.SetAsync(CacheKeyConstants.ChatUsers, users);
        await SendConnectedUser(users);
    }

    public async Task SendConnectedUser(List<ChatUser> users)
    {
        await Clients.All.SendAsync(NotifyMethodConstants.SendConnectedUser, users);
    }

    public async Task UserTyping(string userId, string typingBy, bool isTyping)
    {
        var connectionId = await _cache.GetAsync<string>(CacheKeyConstants.NotifyUser(userId));
        if (!string.IsNullOrEmpty(connectionId))
            await Clients.Client(connectionId).SendAsync(NotifyMethodConstants.ReciveTypingRequest, typingBy, isTyping, DateTime.Now);
    }
}

