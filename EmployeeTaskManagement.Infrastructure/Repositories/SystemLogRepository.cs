using EmployeeTaskManagement.Application.Interfaces.Repositories;
using EmployeeTaskManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTaskManagement.Infrastructure.Repositories;

public class SystemLogRepository : GenericRepository<SystemLog>, ISystemLogRepository
{
    private readonly AppDbContext _context;

    public SystemLogRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SystemLog>> GetLogsByLevelAsync(string level)
    {
        return await _context.SystemLogs
            .Where(log => log.Level == level)
            .OrderByDescending(log => log.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<SystemLog>> GetLogsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.SystemLogs
            .Where(log => log.CreatedAt >= startDate && log.CreatedAt <= endDate)
            .OrderByDescending(log => log.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<SystemLog>> GetRecentLogsAsync(int count)
    {
        return await _context.SystemLogs
            .OrderByDescending(log => log.CreatedAt)
            .Take(count)
            .ToListAsync();
    }
}
