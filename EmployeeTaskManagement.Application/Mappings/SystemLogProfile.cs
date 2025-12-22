using AutoMapper;
using EmployeeTaskManagement.Application.DTOs.SystemLog;
using EmployeeTaskManagement.Domain.Entities;

namespace EmployeeTaskManagement.Application.Mappings;

public class SystemLogProfile : Profile
{
    public SystemLogProfile()
    {
        // SystemLog -> SystemLogResponseDto
        CreateMap<SystemLog, SystemLogResponseDto>();
    }
}
