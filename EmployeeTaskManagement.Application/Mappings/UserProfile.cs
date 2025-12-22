using AutoMapper;
using EmployeeTaskManagement.Application.DTOs.User;
using EmployeeTaskManagement.Domain.Entities;

namespace EmployeeTaskManagement.Application.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        // CreateUserDto -> User
        CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Tasks, opt => opt.Ignore())
            .ForMember(dest => dest.LeaveRequests, opt => opt.Ignore());

        // UpdateUserDto -> User
        CreateMap<UpdateUserDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Tasks, opt => opt.Ignore())
            .ForMember(dest => dest.LeaveRequests, opt => opt.Ignore());

        // User -> UserResponseDto
        CreateMap<User, UserResponseDto>();
    }
}
