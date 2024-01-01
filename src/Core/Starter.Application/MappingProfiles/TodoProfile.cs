using AutoMapper;
using Starter.Application.Features.Todos.Dto;
using Starter.Domain.Entities;

namespace Starter.Application.MappingProfiles;
public class TodoProfile : Profile
{
    public TodoProfile()
    {
        CreateMap<TodoItem, GetToDoItemDto>();
    }
}
