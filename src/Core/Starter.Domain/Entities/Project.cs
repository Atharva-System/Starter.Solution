using System.ComponentModel.DataAnnotations.Schema;

namespace Starter.Domain.Entities;
public sealed class Project : BaseAuditableEntity
{
    public string? ProjectName { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set;}

    [Column(TypeName = "decimal(18,2)")]
    public decimal EstimatedHours { get; set; }
    public ICollection<Tasks> Tasks { get; set; }
}
