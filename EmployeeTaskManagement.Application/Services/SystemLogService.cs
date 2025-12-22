using AutoMapper;
using EmployeeTaskManagement.Application.DTOs.SystemLog;
using EmployeeTaskManagement.Application.Interfaces.Repositories;
using EmployeeTaskManagement.Application.Interfaces.Services;
using EmployeeTaskManagement.Domain.Entities;

namespace EmployeeTaskManagement.Application.Services;

public class SystemLogService : ISystemLogService
{
    private readonly ISystemLogRepository _logRepository;
    private readonly IMapper _mapper;

    public SystemLogService(
        ISystemLogRepository logRepository,
        IMapper mapper)
    {
        _logRepository = logRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SystemLogResponseDto>> GetAllLogsAsync()
    {
        var logs = await _logRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<SystemLogResponseDto>>(logs);
    }

    public async Task<SystemLogResponseDto?> GetLogByIdAsync(int id)
    {
        var log = await _logRepository.GetByIdAsync(id);
        return log == null ? null : _mapper.Map<SystemLogResponseDto>(log);
    }

    public async Task CreateLogAsync(string action, string description, string level)
    {
        var log = new SystemLog
        {
            Action = action,
            Description = description,
            Level = level,
            CreatedAt = DateTime.UtcNow
        };

        await _logRepository.AddAsync(log);
        await _logRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<SystemLogResponseDto>> GetLogsByLevelAsync(string level)
    {
        var logs = await _logRepository.GetLogsByLevelAsync(level);
        return _mapper.Map<IEnumerable<SystemLogResponseDto>>(logs);
    }

    public async Task<IEnumerable<SystemLogResponseDto>> GetLogsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var logs = await _logRepository.GetLogsByDateRangeAsync(startDate, endDate);
        return _mapper.Map<IEnumerable<SystemLogResponseDto>>(logs);
    }

    public async Task<IEnumerable<SystemLogResponseDto>> GetRecentLogsAsync(int count)
    {
        var logs = await _logRepository.GetRecentLogsAsync(count);
        return _mapper.Map<IEnumerable<SystemLogResponseDto>>(logs);
    }
}
