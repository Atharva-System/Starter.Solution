using Starter.Application.Contracts.Responses;
using Starter.Application.Features.Tasks.Dto;

namespace Starter.Application.Features.Tasks.Query.GetTasks;
public class GetProjectListQuery : IRequest<List<ProjectDropdownDto>>
{
    public string userId { get; set; }
}
