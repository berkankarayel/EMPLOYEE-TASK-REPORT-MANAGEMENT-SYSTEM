namespace EmployeeTaskManagement.Domain.Entities;

public class TaskHistory
{
    public int Id { get; set; }

    public int TaskId { get; set; }
    public TaskItem Task { get; set; } = null!;

    public string OldStatus { get; set; } = null!;
    public string NewStatus { get; set; } = null!;

    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
}
