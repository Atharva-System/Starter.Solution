namespace Starter.Blazor.Modules.Projects.Models;

public class ProjectDetailsDto
{
    public string Id { get; set; }
    public string ProjectName { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal EstimatedHours { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
}
