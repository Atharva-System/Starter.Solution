namespace Starter.Application.Features.Tasks.Dto;
public class TaskListDto
{
    public Guid Id { get; set; }
    public string? TaskName { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string StartDateDisplay { get; set; }
    public string EndDateDisplay { get; set; }
    public string Status { get; set; }
    public string StatusDisplay { get; set; }
    public string Priority { get; set; }
    public string PriorityDisplay { get; set; }
    public Guid ProjectId { get; set; }
    public string? ProjectName { get; set; }
    public string? AssignedTo { get; set; }
    public string? AssignedToName { get; set; }
    public DateTime CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public string? ModifiedBy { get; set; }
}
