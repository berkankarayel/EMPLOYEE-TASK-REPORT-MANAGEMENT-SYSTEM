using EmployeeTaskManagement.Application.Interfaces.Repositories;
using EmployeeTaskManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTaskManagement.Infrastructure.Repositories;

public class LeaveRequestRepository : GenericRepository<LeaveRequest>, ILeaveRequestRepository
{
    private readonly AppDbContext _context;

    public LeaveRequestRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<LeaveRequest>> GetLeaveRequestsByUserIdAsync(int userId)
    {
        return await _context.LeaveRequests
            .Include(lr => lr.User)
            .Where(lr => lr.UserId == userId)
            .OrderByDescending(lr => lr.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<LeaveRequest>> GetPendingLeaveRequestsAsync()
    {
        return await _context.LeaveRequests
            .Include(lr => lr.User)
            .Where(lr => lr.Status == "Pending")
            .OrderBy(lr => lr.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<LeaveRequest>> GetLeaveRequestsByStatusAsync(string status)
    {
        return await _context.LeaveRequests
            .Include(lr => lr.User)
            .Where(lr => lr.Status == status)
            .OrderByDescending(lr => lr.CreatedAt)
            .ToListAsync();
    }

    public async Task<LeaveRequest?> GetLeaveRequestWithUserAsync(int leaveRequestId)
    {
        return await _context.LeaveRequests
            .Include(lr => lr.User)
            .FirstOrDefaultAsync(lr => lr.Id == leaveRequestId);
    }
}
