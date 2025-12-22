namespace EmployeeTaskManagement.Application.DTOs.Task;

public class UpdateTaskDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Status { get; set; } = null!; // Pending / Started / Completed
    public DateTime DueDate { get; set; }
    public int AssignedUserId { get; set; }
}
