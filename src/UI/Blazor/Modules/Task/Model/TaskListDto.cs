﻿using System.ComponentModel.DataAnnotations;

namespace Starter.Blazor.Modules.Task.Model;

public class TaskListDto
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Task Name is required.")]
    public string TaskName { get; set; }
    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; }
    [Required(ErrorMessage = "Start Date is required.")]
    public DateTime StartDate { get; set; } = new DateTime();
    [Required(ErrorMessage = "End Date is required.")]
    public DateTime EndDate { get; set; }
    [Required(ErrorMessage = "Status is required.")]
    public int Status { get; set; }
    [Required(ErrorMessage = "Priority is required.")]
    public int Priority { get; set; }
    [Required(ErrorMessage = "Project is required.")]
    public Guid ProjectId { get; set; }
    public string AssignedTo { get; set; }
    public DateTime CreatedOn { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public string ModifiedBy { get; set; }
}
