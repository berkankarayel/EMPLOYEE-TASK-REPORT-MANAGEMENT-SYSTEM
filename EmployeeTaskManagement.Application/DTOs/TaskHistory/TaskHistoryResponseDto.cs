namespace EmployeeTaskManagement.Application.DTOs.TaskHistory;

public class TaskHistoryResponseDto
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public string TaskTitle { get; set; } = null!;
    public string OldStatus { get; set; } = null!;
    public string NewStatus { get; set; } = null!;
    public DateTime ChangedAt { get; set; }
}
