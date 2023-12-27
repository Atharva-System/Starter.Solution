using Starter.Application.Features.Common;

namespace Starter.Application.Features.Projects.Command.UpdateProject;
public sealed record UpdateProjectCommand : IRequest<ApiResponse<int>>
{
    public Guid Id { get; set; }
    public string? ProjectName { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TimeOnly EstimatedHours { get; set; }
}
