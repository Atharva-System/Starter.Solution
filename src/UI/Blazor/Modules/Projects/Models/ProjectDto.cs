namespace Starter.Blazor.Modules.Projects.Models;

public class ProjectDto
{
    public string Id { get; set; }
    public string ProjectName { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string StartDateDisplay { get; set; }
    public string EndDateDisplay { get; set; }
    public decimal EstimatedHours { get; set; }
}
