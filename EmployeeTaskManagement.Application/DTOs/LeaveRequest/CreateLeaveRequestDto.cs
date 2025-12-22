namespace EmployeeTaskManagement.Application.DTOs.LeaveRequest;

public class CreateLeaveRequestDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Reason { get; set; } = null!;
}
