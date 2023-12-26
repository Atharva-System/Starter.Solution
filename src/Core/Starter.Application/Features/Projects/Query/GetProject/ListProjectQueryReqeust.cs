using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Starter.Application.Contracts.Responses;
using Starter.Application.Features.Common;
using Starter.Application.Features.Projects.Dtos;
using Starter.Application.Features.Todos.Create;
using Starter.Application.Models.Specification.Filters;
using Starter.Application.UnitOfWork;
using Starter.Domain.Entities;
using Starter.Domain.Enums;
using Starter.Domain.Events;

namespace Starter.Application.Features.Projects.Query.GetProject;
public sealed record ListProjectQueryReqeust : IRequest<IPagedDataResponse<ProjectListDto>>
{
    public PaginationFilter PaginationFilter { get; set; } = default!;
}
