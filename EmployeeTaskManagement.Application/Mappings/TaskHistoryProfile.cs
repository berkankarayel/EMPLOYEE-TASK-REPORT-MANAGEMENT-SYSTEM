using AutoMapper;
using EmployeeTaskManagement.Application.DTOs.TaskHistory;
using EmployeeTaskManagement.Domain.Entities;

namespace EmployeeTaskManagement.Application.Mappings;

public class TaskHistoryProfile : Profile
{
    public TaskHistoryProfile()
    {
        // TaskHistory -> TaskHistoryResponseDto
        CreateMap<TaskHistory, TaskHistoryResponseDto>()
            .ForMember(dest => dest.TaskTitle, opt => opt.MapFrom(src => src.Task.Title));
    }
}
