using AutoMapper;
using EmployeeTaskManagement.Application.DTOs.Task;
using EmployeeTaskManagement.Application.Interfaces.Repositories;
using EmployeeTaskManagement.Application.Interfaces.Services;
using EmployeeTaskManagement.Domain.Entities;

namespace EmployeeTaskManagement.Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUserRepository _userRepository;
    private readonly ISystemLogRepository _logRepository;
    private readonly IMapper _mapper;

    public TaskService(
        ITaskRepository taskRepository,
        IUserRepository userRepository,
        ISystemLogRepository logRepository,
        IMapper mapper)
    {
        _taskRepository = taskRepository;
        _userRepository = userRepository;
        _logRepository = logRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TaskResponseDto>> GetAllTasksAsync()
    {
        var tasks = await _taskRepository.GetAllWithIncludesAsync(t => t.AssignedUser);
        return _mapper.Map<IEnumerable<TaskResponseDto>>(tasks);
    }

    public async Task<IEnumerable<TaskResponseDto>> GetTasksByUserIdAsync(int userId)
    {
        var tasks = await _taskRepository.GetTasksByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<TaskResponseDto>>(tasks);
    }

    public async Task<TaskResponseDto?> GetTaskByIdAsync(int id)
    {
        var task = await _taskRepository.GetTaskWithUserAsync(id);
        return task == null ? null : _mapper.Map<TaskResponseDto>(task);
    }

    public async Task<TaskResponseDto> CreateTaskAsync(CreateTaskDto createTaskDto)
    {
        // Kullanıcı kontrolü
        var user = await _userRepository.GetByIdAsync(createTaskDto.AssignedUserId);
        if (user == null)
        {
            throw new InvalidOperationException("Atanan kullanıcı bulunamadı.");
        }

        var task = _mapper.Map<TaskItem>(createTaskDto);
        task.CreatedAt = DateTime.UtcNow;

        await _taskRepository.AddAsync(task);
        await _taskRepository.SaveChangesAsync();

        // Task History oluştur
        await CreateTaskHistoryAsync(task.Id, "", "Pending");

        await LogAsync("Task Created", $"New task created: {task.Title}, Assigned to: {user.FullName}", "Info");

        var result = await _taskRepository.GetTaskWithUserAsync(task.Id);
        return _mapper.Map<TaskResponseDto>(result);
    }

    public async Task<TaskResponseDto?> UpdateTaskAsync(UpdateTaskDto updateTaskDto)
    {
        var task = await _taskRepository.GetByIdAsync(updateTaskDto.Id);
        if (task == null)
        {
            return null;
        }

        var oldStatus = task.Status;
        _mapper.Map(updateTaskDto, task);

        await _taskRepository.UpdateAsync(task);
        await _taskRepository.SaveChangesAsync();

        // Status değiştiyse history oluştur
        if (oldStatus != updateTaskDto.Status)
        {
            await CreateTaskHistoryAsync(task.Id, oldStatus, updateTaskDto.Status);
        }

        await LogAsync("Task Updated", $"Task updated: {task.Title}", "Info");

        var result = await _taskRepository.GetTaskWithUserAsync(task.Id);
        return _mapper.Map<TaskResponseDto>(result);
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null)
        {
            return false;
        }

        await _taskRepository.DeleteAsync(id);
        await _taskRepository.SaveChangesAsync();

        await LogAsync("Task Deleted", $"Task deleted: {task.Title}", "Warning");

        return true;
    }

    public async Task<IEnumerable<TaskResponseDto>> GetTasksByStatusAsync(string status)
    {
        var tasks = await _taskRepository.GetTasksByStatusAsync(status);
        return _mapper.Map<IEnumerable<TaskResponseDto>>(tasks);
    }

    public async Task<IEnumerable<TaskResponseDto>> GetOverdueTasksAsync()
    {
        var tasks = await _taskRepository.GetOverdueTasksAsync();
        return _mapper.Map<IEnumerable<TaskResponseDto>>(tasks);
    }

    public async Task<bool> UpdateTaskStatusAsync(int taskId, string newStatus)
    {
        var task = await _taskRepository.GetByIdAsync(taskId);
        if (task == null)
        {
            return false;
        }

        var oldStatus = task.Status;
        task.Status = newStatus;

        await _taskRepository.UpdateAsync(task);
        await _taskRepository.SaveChangesAsync();

        await CreateTaskHistoryAsync(taskId, oldStatus, newStatus);

        await LogAsync("Task Status Updated", $"Task '{task.Title}' status changed: {oldStatus} -> {newStatus}", "Info");

        return true;
    }

    private async Task CreateTaskHistoryAsync(int taskId, string oldStatus, string newStatus)
    {
        var history = new TaskHistory
        {
            TaskId = taskId,
            OldStatus = oldStatus,
            NewStatus = newStatus,
            ChangedAt = DateTime.UtcNow
        };

        // TaskHistory için repository gerekirse eklenecek
        // Şimdilik direkt DbContext üzerinden eklenebilir
    }

    private async Task LogAsync(string action, string description, string level)
    {
        await _logRepository.AddAsync(new SystemLog
        {
            Action = action,
            Description = description,
            Level = level,
            CreatedAt = DateTime.UtcNow
        });
        await _logRepository.SaveChangesAsync();
    }
}
