using AutoMapper;
using Starter.Application.Features.Tasks.Dto;
using Starter.Domain.Entities;
using Starter.Domain.Enums;

namespace Starter.Application.MappingProfiles;
public class TaskProfile : Profile
{
    public TaskProfile()
    {
        CreateMap<Tasks, TaskDetailsDto>()
            .ForMember(m => m.StatusName, opt => opt.MapFrom(x => ((TasksStatus)x.Status).ToString()))
            .ForMember(m => m.PriorityName, opt => opt.MapFrom(x => ((TaskPriority)x.Priority).ToString()));
    }
}
