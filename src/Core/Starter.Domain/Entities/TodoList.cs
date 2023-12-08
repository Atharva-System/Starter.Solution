namespace Starter.Domain.Entities;

public sealed class TodoList : BaseAuditableEntity
{
    public string? Title { get; set; }
    public IList<TodoItem> Items { get; private set; } = new List<TodoItem>();
}
