using Starter.Application.Features.Common;
using Starter.Domain.Events;

namespace Starter.Application.Features.Tasks.Command.CreateCommand;
public sealed class TaskCreatedDomainEventHandler(INotifyService notificationService) : INotificationHandler<TaskCreatedDomainEvent>
{
    public readonly INotifyService _notificationService = notificationService;

    public async Task Handle(TaskCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _notificationService.SendToClientAsync(NotifyMethodConstants.ReceiveTaskCreation,
            new TaskCreatedParam
            {
                TaskId = notification.TaskId,
                TaskName = notification.TaskName
            },
            notification.UserId, cancellationToken);
    }
}

public class TaskCreatedParam : INotifyParam
{
    public string TaskId { get; set; }
    public string TaskName { get; set; }
}
