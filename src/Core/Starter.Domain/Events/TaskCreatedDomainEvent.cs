namespace Starter.Domain.Events;
public record TaskCreatedDomainEvent(string UserId, string TaskId, string TaskName) : BaseEvent;

