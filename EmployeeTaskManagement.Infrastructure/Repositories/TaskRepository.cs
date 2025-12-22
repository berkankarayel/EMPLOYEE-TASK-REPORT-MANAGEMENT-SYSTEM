using EmployeeTaskManagement.Application.Interfaces.Repositories;
using EmployeeTaskManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTaskManagement.Infrastructure.Repositories;

public class TaskRepository : GenericRepository<TaskItem>, ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TaskItem>> GetTasksByUserIdAsync(int userId)
    {
        return await _context.Tasks
            .Include(t => t.AssignedUser)
            .Where(t => t.AssignedUserId == userId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<TaskItem>> GetTasksByStatusAsync(string status)
    {
        return await _context.Tasks
            .Include(t => t.AssignedUser)
            .Where(t => t.Status == status)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<TaskItem>> GetOverdueTasksAsync()
    {
        return await _context.Tasks
            .Include(t => t.AssignedUser)
            .Where(t => t.DueDate < DateTime.UtcNow && t.Status != "Completed")
            .OrderBy(t => t.DueDate)
            .ToListAsync();
    }

    public async Task<TaskItem?> GetTaskWithUserAsync(int taskId)
    {
        return await _context.Tasks
            .Include(t => t.AssignedUser)
            .FirstOrDefaultAsync(t => t.Id == taskId);
    }
}
