using Starter.Application.Features.Common;
using Starter.Application.Features.Tasks.Dto;

namespace Starter.Application.Features.Tasks.Query;
public record GetProjectListQuery : IRequest<ApiResponse<List<ProjectDropdownDto>>>;
