namespace Starter.Domain.Events;
public record TodoItemCreatedEvent(TodoItem item) : BaseEvent;
