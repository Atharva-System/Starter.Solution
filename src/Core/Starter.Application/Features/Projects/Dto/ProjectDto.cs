using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starter.Application.Features.Projects.Dto;
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
