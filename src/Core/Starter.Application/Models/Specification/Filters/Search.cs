namespace Starter.Application.Models.Specification.Filters;
public class Search
{
    public List<string> Fields { get; set; } = new();
    public string? Keyword { get; set; }
}
