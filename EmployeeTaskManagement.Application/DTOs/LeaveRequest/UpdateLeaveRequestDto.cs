namespace EmployeeTaskManagement.Application.DTOs.LeaveRequest;

public class UpdateLeaveRequestDto
{
    public int Id { get; set; }
    public string Status { get; set; } = null!; // Pending / Approved / Rejected
}
