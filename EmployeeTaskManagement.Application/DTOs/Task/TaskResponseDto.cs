namespace EmployeeTaskManagement.Application.DTOs.Task;

public class TaskResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTime DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public int AssignedUserId { get; set; }
    public string AssignedUserName { get; set; } = null!;
}
