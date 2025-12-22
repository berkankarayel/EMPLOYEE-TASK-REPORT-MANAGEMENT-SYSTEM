using EmployeeTaskManagement.Application.DTOs.Task;
using EmployeeTaskManagement.Application.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeeTaskManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;
    private readonly IValidator<CreateTaskDto> _createValidator;
    private readonly IValidator<UpdateTaskDto> _updateValidator;

    public TaskController(
        ITaskService taskService,
        IValidator<CreateTaskDto> createValidator,
        IValidator<UpdateTaskDto> updateValidator)
    {
        _taskService = taskService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTasks()
    {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

        if (userRole == "Admin")
        {
            var tasks = await _taskService.GetAllTasksAsync();
            return Ok(tasks);
        }
        else
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var tasks = await _taskService.GetTasksByUserIdAsync(userId);
            return Ok(tasks);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaskById(int id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);
        if (task == null)
        {
            return NotFound(new { message = "Görev bulunamadı." });
        }

        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

        if (userRole != "Admin" && task.AssignedUserId != userId)
        {
            return Forbid();
        }

        return Ok(task);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto createTaskDto)
    {
        var validationResult = await _createValidator.ValidateAsync(createTaskDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        try
        {
            var task = await _taskService.CreateTaskAsync(createTaskDto);
            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskDto updateTaskDto)
    {
        if (id != updateTaskDto.Id)
        {
            return BadRequest(new { message = "ID uyuşmazlığı." });
        }

        var validationResult = await _updateValidator.ValidateAsync(updateTaskDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        try
        {
            var task = await _taskService.UpdateTaskAsync(updateTaskDto);
            if (task == null)
            {
                return NotFound(new { message = "Görev bulunamadı." });
            }
            return Ok(task);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var result = await _taskService.DeleteTaskAsync(id);
        if (!result)
        {
            return NotFound(new { message = "Görev bulunamadı." });
        }
        return NoContent();
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] string status)
    {
        var result = await _taskService.UpdateTaskStatusAsync(id, status);
        if (!result)
        {
            return NotFound(new { message = "Görev bulunamadı." });
        }
        return Ok(new { message = "Görev durumu güncellendi." });
    }

    [HttpGet("overdue")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetOverdueTasks()
    {
        var tasks = await _taskService.GetOverdueTasksAsync();
        return Ok(tasks);
    }
}
