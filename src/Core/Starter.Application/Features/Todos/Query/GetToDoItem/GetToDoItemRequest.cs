using AutoMapper;
using Starter.Application.Exceptions;
using Starter.Application.Features.Common;
using Starter.Application.Features.Todos.Dto;
using Starter.Application.UnitOfWork;

namespace Starter.Application.Features.Todos.Query;
public record GetToDoItemRequest(Guid id) : IRequest<ApiResponse<GetToDoItemDto>>;

public sealed class GetToDoItemRequestHandler(IQueryUnitOfWork queryRepository, IMapper mapper) : IRequestHandler<GetToDoItemRequest, ApiResponse<GetToDoItemDto>>
{
    private readonly IQueryUnitOfWork _queryRepository = queryRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ApiResponse<GetToDoItemDto>> Handle(GetToDoItemRequest request, CancellationToken cancellationToken)
    {
        var todoItem = await _queryRepository.TodoQuery.GetByGUIdAsync(request.id);
        _ = todoItem ?? throw new NotFoundException("TodoId ", request.id);

        var todoDto = _mapper.Map<GetToDoItemDto>(todoItem);

        //var todoItem1 = await _queryRepository.QueryRepository<TodoItem>().GetByIdAsync(request.id.ToString());

        return new ApiResponse<GetToDoItemDto> { Data = todoDto };
    }
}
