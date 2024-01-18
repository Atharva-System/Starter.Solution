using System.ComponentModel.DataAnnotations;
using Starter.Blazor.Modules.Common;

namespace Starter.Blazor.Modules.Task.Model;

public class TaskListDto
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Task Name is required.")]
    public string TaskName { get; set; }
    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Required(ErrorMessage = "Start Date is required.")]
    [DateInFuture(ErrorMessage = "Start Date must be in the future.")]
    public DateTime StartDate { get; set; } = new DateTime();
    [Required(ErrorMessage = "End Date is required.")]
    public DateTime EndDate { get; set; }
    [Required(ErrorMessage = "Status is required.")]
    public int? Status { get; set; }
    [Required(ErrorMessage = "Priority is required.")]
    public int? Priority { get; set; }
    [Required(ErrorMessage = "Project is required.")]
    public Guid? ProjectId { get; set; }
    [Required(ErrorMessage = "Assignee is required.")]
    public string AssignedTo { get; set; }
    public DateTime CreatedOn { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public string ModifiedBy { get; set; }
    public string StartDateDisplay { get; set; }
    public string EndDateDisplay { get; set; }
    public string StatusDisplay { get; set; }
    public string PriorityDisplay { get; set; }
    public string? ProjectName { get; set; }
    public string? AssignedToName { get; set; }
}
