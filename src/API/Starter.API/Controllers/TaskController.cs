﻿using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Starter.API.Controllers.Base;
using Starter.Application.Contracts.Responses;
using Starter.Application.Extensions;
using Starter.Application.Features.Common;
using Starter.Application.Features.Tasks.Command;
using Starter.Application.Features.Tasks.Command.UpdateTask;
using Starter.Application.Features.Tasks.Dto;
using Starter.Application.Features.Tasks.Query;
using Starter.Application.Models.Specification.Filters;
using Starter.Application.Models.Task.Dto;
using Starter.Identity.Authorizations;
using Starter.Identity.Authorizations.Permissions;
using Action = Starter.Identity.Authorizations.Action;

namespace Starter.API.Controllers;

public class TaskController : BaseApiController
{
    [HttpPost("Create")]
    [MustHavePermission(Action.Create, Resource.Task)]
    public async Task<ApiResponse<int>> CreateTask(CreateTaskCommandRequest request)
    {
        return await Mediator.Send(request);
    }

    [HttpGet("{id}")]
    [MustHavePermission(Action.View, Resource.Task)]
    public async Task<ApiResponse<TaskDetailsDto>> GetTaskDetails(Guid id)
    {
        return await Mediator.Send(new GetTaskDetailsQueryRequest(id));
    }

    [HttpDelete("{id}")]
    [MustHavePermission(Action.Delete, Resource.Task)]
    public async Task<ApiResponse<string>> DeleteTask(Guid id)
    {
        return await Mediator.Send(new DeleteTaskCommandRequest(id));
    }

    [HttpPut("{id}")]
    [MustHavePermission(Action.Update, Resource.Task)]
    public async Task<ApiResponse<string>> UpdateTask(Guid id, UpdateTaskRequestCommand request)
    {
        if (id != request.Id)
        {
            return new ApiResponse<string>
            {
                Success = false,
                Data = "The provided ID in the route does not match the ID in the request body.",
                StatusCode = HttpStatusCodes.BadRequest
            };
        }
        return await Mediator.Send(request);
    }

    [HttpPost("Search")]
    [MustHavePermission(Action.View, Resource.Task)]
    public async Task<IPagedDataResponse<TaskListDto>> GetListAsync(PaginationFilter request)
    {
        return await Mediator.Send(new GetTasksQuery() { PaginationFilter = request });
    }

    [HttpGet("status-list")]
    [MustHavePermission(Action.View, Resource.Task)]
    public ApiResponse<List<EnumTypeViewDto>> GetStatusAsync()
    {
        return new ApiResponse<List<EnumTypeViewDto>>
        {
            Success = true,
            StatusCode = HttpStatusCodes.OK,
            Data = CommonFunction.GetTaskStatusList(),
        };
    }

    [HttpGet("priority-list")]
    [MustHavePermission(Action.View, Resource.Task)]
    public ApiResponse<List<EnumTypeViewDto>> GetPrioritiesAsync()
    {
        return new ApiResponse<List<EnumTypeViewDto>>
        {
            Success = true,
            StatusCode = HttpStatusCodes.OK,
            Data = CommonFunction.GetTaskPriorityList(),
        };
    }

    [HttpGet("assignee-list")]
    [MustHavePermission(Action.View, Resource.Task)]
    public async Task<ApiResponse<List<TaskAssigneeDto>>> GetAssigneeListAsync()
    {
        return await Mediator.Send(new GetTaskAssigneesQueryRequest());
    }

    [HttpGet("projects")]
    [MustHavePermission(Action.View, Resource.Task)]
    public async Task<ApiResponse<List<ProjectDropdownDto>>> GetProjectListAsync()
    {
        return await Mediator.Send(new GetProjectListQuery());
    }
}
