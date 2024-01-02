namespace Starter.Application.Features.Tasks.Dto;
public class TaskDetailsDto
{
    public Guid Id { get; set; }
    public string? TaskName { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Status { get; set; }
    public int Priority { get; set; }
    public string? StatusName { get; set; }
    public string? PriorityName { get; set; }
    public Guid ProjectId { get; set; }
    public string? AssignedTo { get; set; }
    public string? AssignedToName { get;set; }
}
