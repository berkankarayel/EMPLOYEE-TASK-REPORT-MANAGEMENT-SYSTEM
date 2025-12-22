using EmployeeTaskManagement.Application.DTOs.SystemLog;

namespace EmployeeTaskManagement.Application.Interfaces.Services;

public interface ISystemLogService
{
    Task<IEnumerable<SystemLogResponseDto>> GetAllLogsAsync();
    Task<SystemLogResponseDto?> GetLogByIdAsync(int id);
    Task CreateLogAsync(string action, string description, string level);
    Task<IEnumerable<SystemLogResponseDto>> GetLogsByLevelAsync(string level);
    Task<IEnumerable<SystemLogResponseDto>> GetLogsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<SystemLogResponseDto>> GetRecentLogsAsync(int count);
}
