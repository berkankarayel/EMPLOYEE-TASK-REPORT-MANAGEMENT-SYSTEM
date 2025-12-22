namespace EmployeeTaskManagement.Domain.Entities;

public class TaskItem
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Status { get; set; } = null!; // Pending / Started / Completed

    public DateTime DueDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign Keys
    public int AssignedUserId { get; set; }
    public User AssignedUser { get; set; } = null!;

    // Navigation
    public ICollection<TaskHistory> TaskHistories { get; set; } = new List<TaskHistory>();
}
