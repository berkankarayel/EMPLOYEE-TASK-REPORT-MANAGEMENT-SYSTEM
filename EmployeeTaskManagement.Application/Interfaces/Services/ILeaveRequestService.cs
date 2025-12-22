using EmployeeTaskManagement.Application.DTOs.LeaveRequest;

namespace EmployeeTaskManagement.Application.Interfaces.Services;

public interface ILeaveRequestService
{
    Task<IEnumerable<LeaveRequestResponseDto>> GetAllLeaveRequestsAsync();
    Task<LeaveRequestResponseDto?> GetLeaveRequestByIdAsync(int id);
    Task<LeaveRequestResponseDto> CreateLeaveRequestAsync(CreateLeaveRequestDto createLeaveRequestDto, int userId);
    Task<LeaveRequestResponseDto?> UpdateLeaveRequestStatusAsync(int id, string status);
    Task<IEnumerable<LeaveRequestResponseDto>> GetLeaveRequestsByUserIdAsync(int userId);
    Task<IEnumerable<LeaveRequestResponseDto>> GetPendingLeaveRequestsAsync();
    Task<IEnumerable<LeaveRequestResponseDto>> GetLeaveRequestsByStatusAsync(string status);
}
