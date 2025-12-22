using AutoMapper;
using EmployeeTaskManagement.Application.DTOs.Task;
using EmployeeTaskManagement.Domain.Entities;

namespace EmployeeTaskManagement.Application.Mappings;

public class TaskProfile : Profile
{
    public TaskProfile()
    {
        // CreateTaskDto -> TaskItem
        CreateMap<CreateTaskDto, Domain.Entities.TaskItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Pending"))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.AssignedUser, opt => opt.Ignore())
            .ForMember(dest => dest.TaskHistories, opt => opt.Ignore());

        // UpdateTaskDto -> TaskItem
        CreateMap<UpdateTaskDto, Domain.Entities.TaskItem>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.AssignedUser, opt => opt.Ignore())
            .ForMember(dest => dest.TaskHistories, opt => opt.Ignore());

        // TaskItem -> TaskResponseDto
        CreateMap<Domain.Entities.TaskItem, TaskResponseDto>()
            .ForMember(dest => dest.AssignedUserName, opt => opt.MapFrom(src => src.AssignedUser.FullName));
    }
}
