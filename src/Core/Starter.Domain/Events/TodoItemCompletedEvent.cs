namespace Starter.Domain.Events;

public record TodoItemCompletedEvent(TodoItem item) : BaseEvent;
