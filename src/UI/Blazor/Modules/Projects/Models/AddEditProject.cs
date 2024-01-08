using System.ComponentModel.DataAnnotations;

namespace Starter.Blazor.Modules.Projects.Models;

public class AddEditProject
{
    public string? Id { get; set; }
    [Required(ErrorMessage = "Project Name is required")]
    public string ProjectName { get; set; }
    public string? Description { get; set; }
    [Required(ErrorMessage = "Start Date is required")]
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    [Required(ErrorMessage = "End Date is required")]
    public DateTime EndDate { get; set; } = DateTime.UtcNow;
    [Required(ErrorMessage = "Estimated Hours is required")]
    public decimal EstimatedHours { get; set; }
}
