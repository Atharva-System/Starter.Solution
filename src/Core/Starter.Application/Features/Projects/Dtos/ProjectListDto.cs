using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starter.Application.Features.Projects.Dtos;
public class ProjectListDto
{
    public Guid Id { get; set; } = default!;
    public string? ProjectName { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string StartDateDisplay { get; set; }
    public string EndDateDisplay { get; set; }
    public decimal EstimatedHours { get; set; }
}
