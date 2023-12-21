namespace Starter.Domain.Entities;
public sealed class Project : BaseAuditableEntity
{
    public string? ProjectName { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set;}
    public TimeOnly EstimatedHours { get; set; }
}
