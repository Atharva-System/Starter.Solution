namespace Starter.Blazor.Modules.Task.Model;

public class TaskDetailsDto
{
    public string? TaskName { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Status { get; set; }
    public string Priority { get; set; }
    public Guid ProjectId { get; set; }
    public string? AssignedTo { get; set; }
}
