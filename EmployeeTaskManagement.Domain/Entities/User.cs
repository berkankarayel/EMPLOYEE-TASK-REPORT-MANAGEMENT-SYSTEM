namespace EmployeeTaskManagement.Domain.Entities;

public class User
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!; // Admin / User

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
}
