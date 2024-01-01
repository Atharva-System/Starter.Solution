namespace Starter.Application.Features.Todos.Dto;
public class GetToDoItemDto
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public string? Note { get; set; } = string.Empty;
}
