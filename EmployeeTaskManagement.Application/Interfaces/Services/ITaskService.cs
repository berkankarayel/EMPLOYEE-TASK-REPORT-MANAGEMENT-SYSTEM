using EmployeeTaskManagement.Application.DTOs.Task;

namespace EmployeeTaskManagement.Application.Interfaces.Services;

public interface ITaskService
{
    Task<IEnumerable<TaskResponseDto>> GetAllTasksAsync();
    Task<TaskResponseDto?> GetTaskByIdAsync(int id);
    Task<TaskResponseDto> CreateTaskAsync(CreateTaskDto createTaskDto);
    Task<TaskResponseDto?> UpdateTaskAsync(UpdateTaskDto updateTaskDto);
    Task<bool> DeleteTaskAsync(int id);
    Task<IEnumerable<TaskResponseDto>> GetTasksByUserIdAsync(int userId);
    Task<IEnumerable<TaskResponseDto>> GetTasksByStatusAsync(string status);
    Task<IEnumerable<TaskResponseDto>> GetOverdueTasksAsync();
    Task<bool> UpdateTaskStatusAsync(int taskId, string newStatus);
}
