namespace Starter.Domain.Entities;
public sealed class Tasks : BaseAuditableEntity
{
    public string? TaskName { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Status { get; set; }
    public int Priority { get; set; }
    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }
    public string? AssignedTo { get; set;}
}
