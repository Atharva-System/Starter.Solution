using Starter.Application.Contracts.Application;
using Starter.Application.Features.Common;
using Starter.Application.Models.Users;

namespace Starter.Application.Features.Users.SendMessage;
public sealed class SendMessageRequest : IRequest
{
    public string? UserId { get; set; }
    public string? Message { get; set; }
}

public class SendMessageCommandHandler(INotifyService notificationService, ICurrentUserService currentUserService) : IRequestHandler<SendMessageRequest>
{
    public readonly INotifyService _notificationService = notificationService;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task Handle(SendMessageRequest request, CancellationToken cancellationToken)
    {
        await _notificationService.SendToClientAsync(NotifyMethodConstants.ReceiveChatMessage,
                   new ChatMessage
                   {
                       FromUserId = request.UserId!,
                       ToUserId = _currentUserService.UserId!,
                       Text = request.Message!,
                       Time = "Just now",
                       Date = DateTime.Now
                   },
                   request.UserId!, cancellationToken);
    }
}
