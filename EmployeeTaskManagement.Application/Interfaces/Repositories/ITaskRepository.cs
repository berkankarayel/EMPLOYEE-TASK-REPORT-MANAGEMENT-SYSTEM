using EmployeeTaskManagement.Domain.Entities;

namespace EmployeeTaskManagement.Application.Interfaces.Repositories;

public interface ITaskRepository : IGenericRepository<TaskItem>
{
    Task<IEnumerable<TaskItem>> GetTasksByUserIdAsync(int userId);
    Task<IEnumerable<TaskItem>> GetTasksByStatusAsync(string status);
    Task<IEnumerable<TaskItem>> GetOverdueTasksAsync();
    Task<TaskItem?> GetTaskWithUserAsync(int taskId);
}
