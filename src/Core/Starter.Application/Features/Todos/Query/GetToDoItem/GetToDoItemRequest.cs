using Starter.Application.Features.Common;
using Starter.Application.Features.Todos.Dto;
using Starter.Application.UnitOfWork;

namespace Starter.Application.Features.Todos.Query.GetToDoItem;
public record GetToDoItemRequest(Guid id) : IRequest<ApiResponse<GetToDoItemDto>>;

public sealed class GetToDoItemRequestHandler : IRequestHandler<GetToDoItemRequest, ApiResponse<GetToDoItemDto>>
{
    private readonly IQueryUnitOfWork _queryRepository;

    public GetToDoItemRequestHandler(IQueryUnitOfWork queryRepository)
    {
        _queryRepository = queryRepository;
    }

    public async Task<ApiResponse<GetToDoItemDto>> Handle(GetToDoItemRequest request, CancellationToken cancellationToken)
    {
        var todoItem = await _queryRepository.TodoQuery.GetByGUIdAsync(request.id);

        //var todoItem1 = await _queryRepository.QueryRepository<TodoItem>().GetByIdAsync(request.id.ToString());

        return new ApiResponse<GetToDoItemDto> { Data = new GetToDoItemDto() { Id = todoItem.Id, Note = todoItem.Note, Title = todoItem.Title } };
    }
}
