using EmployeeTaskManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTaskManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class SystemLogController : ControllerBase
{
    private readonly ISystemLogService _logService;

    public SystemLogController(ISystemLogService logService)
    {
        _logService = logService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllLogs()
    {
        var logs = await _logService.GetAllLogsAsync();
        return Ok(logs);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLogById(int id)
    {
        var log = await _logService.GetLogByIdAsync(id);
        if (log == null)
        {
            return NotFound(new { message = "Log kaydı bulunamadı." });
        }
        return Ok(log);
    }

    [HttpGet("level/{level}")]
    public async Task<IActionResult> GetLogsByLevel(string level)
    {
        var logs = await _logService.GetLogsByLevelAsync(level);
        return Ok(logs);
    }

    [HttpGet("recent/{count}")]
    public async Task<IActionResult> GetRecentLogs(int count)
    {
        var logs = await _logService.GetRecentLogsAsync(count);
        return Ok(logs);
    }

    [HttpGet("daterange")]
    public async Task<IActionResult> GetLogsByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var logs = await _logService.GetLogsByDateRangeAsync(startDate, endDate);
        return Ok(logs);
    }
}
