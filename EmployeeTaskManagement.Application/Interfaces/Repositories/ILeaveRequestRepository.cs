using EmployeeTaskManagement.Domain.Entities;

namespace EmployeeTaskManagement.Application.Interfaces.Repositories;

public interface ILeaveRequestRepository : IGenericRepository<LeaveRequest>
{
    Task<IEnumerable<LeaveRequest>> GetLeaveRequestsByUserIdAsync(int userId);
    Task<IEnumerable<LeaveRequest>> GetPendingLeaveRequestsAsync();
    Task<IEnumerable<LeaveRequest>> GetLeaveRequestsByStatusAsync(string status);
    Task<LeaveRequest?> GetLeaveRequestWithUserAsync(int leaveRequestId);
}
