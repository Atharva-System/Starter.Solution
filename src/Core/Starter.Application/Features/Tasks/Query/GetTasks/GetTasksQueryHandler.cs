﻿using MediatR;
using Starter.Application.Contracts.Responses;
using Starter.Application.Features.Common;
using Starter.Application.Features.Tasks.Dto;
using Starter.Application.Features.Tasks.Query;
using Starter.Application.Models.Task;
using Starter.Application.UnitOfWork;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Starter.Application.Features.Tasks.Query
{
    public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, IPagedDataResponse<TaskListDto>>
    {
        private readonly IQueryUnitOfWork _query;

        public GetTasksQueryHandler(IQueryUnitOfWork query)
        {
            _query = query;
        }

        public async Task<IPagedDataResponse<TaskListDto>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {
            var spec = new GetTasksRequestSpec(request.Filter);

            var response = await _query._taskQueryRepository.SearchAsync(spec, request.Filter.PageNumber, request.Filter.PageSize, cancellationToken);

            return response;
        }

       
    }
}
