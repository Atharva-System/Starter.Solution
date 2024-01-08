﻿using Starter.Blazor.Modules.Common;
using Starter.Blazor.Modules.Projects.Models;

namespace Starter.Blazor.Modules.Projects.Services;

public interface IProjectService
{
    Task<List<ProjectDto>> GetProjectlistsAsync(PaginationRequest param);
}