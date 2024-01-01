using Starter.Application.Features.Common;

namespace Starter.Application.Features.Tasks.Command.UpdateTask;
public sealed record UpdateTaskRequestCommand : IRequest<ApiResponse<string>>
{
    public Guid Id { get; set; }
    public string? TaskName { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Status { get; set; }
    public int Priority { get; set; }
    public Guid ProjectId { get; set; }
    public string? AssignedTo { get; set; }
}
