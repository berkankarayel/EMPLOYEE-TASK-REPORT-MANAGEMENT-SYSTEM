namespace EmployeeTaskManagement.Domain.Entities;

public class LeaveRequest
{
    public int Id { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public string Reason { get; set; } = null!;

    public string Status { get; set; } = null!; // Pending / Approved / Rejected

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign Key
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
