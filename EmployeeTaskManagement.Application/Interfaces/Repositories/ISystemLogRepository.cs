using EmployeeTaskManagement.Domain.Entities;

namespace EmployeeTaskManagement.Application.Interfaces.Repositories;

public interface ISystemLogRepository : IGenericRepository<SystemLog>
{
    Task<IEnumerable<SystemLog>> GetLogsByLevelAsync(string level);
    Task<IEnumerable<SystemLog>> GetLogsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<SystemLog>> GetRecentLogsAsync(int count);
}
