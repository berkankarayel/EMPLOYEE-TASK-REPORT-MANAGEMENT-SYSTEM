namespace EmployeeTaskManagement.Domain.Entities;

public class SystemLog
{
    public int Id { get; set; }

    public string Action { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Level { get; set; } = null!; // Info / Warning / Error

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
