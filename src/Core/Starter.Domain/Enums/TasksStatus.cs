using System.ComponentModel;

namespace Starter.Domain.Enums;
public enum TasksStatus
{
    [Description("To Do")]
    ToDo = 1,

    [Description("In Progress")]
    InProgress = 2,

    [Description("Completed")]
    Completed = 3,
}
