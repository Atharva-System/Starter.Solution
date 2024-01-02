
namespace Starter.Application.Features.Projects.Dtos;
public class ProjectDto
{
    public Guid Id { get; set; }

    public string ProjectName { get; set; } = default!;

    public string Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public TimeOnly EstimatedHours { get; set; }

    public DateTime CreatedOn { get; set; }
}
