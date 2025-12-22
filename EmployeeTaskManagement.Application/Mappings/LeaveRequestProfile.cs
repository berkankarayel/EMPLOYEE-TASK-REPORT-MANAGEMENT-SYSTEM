using AutoMapper;
using EmployeeTaskManagement.Application.DTOs.LeaveRequest;
using EmployeeTaskManagement.Domain.Entities;

namespace EmployeeTaskManagement.Application.Mappings;

public class LeaveRequestProfile : Profile
{
    public LeaveRequestProfile()
    {
        // CreateLeaveRequestDto -> LeaveRequest
        CreateMap<CreateLeaveRequestDto, LeaveRequest>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Pending"))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());

        // UpdateLeaveRequestDto -> LeaveRequest
        CreateMap<UpdateLeaveRequestDto, LeaveRequest>()
            .ForMember(dest => dest.StartDate, opt => opt.Ignore())
            .ForMember(dest => dest.EndDate, opt => opt.Ignore())
            .ForMember(dest => dest.Reason, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());

        // LeaveRequest -> LeaveRequestResponseDto
        CreateMap<LeaveRequest, LeaveRequestResponseDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName));
    }
}
