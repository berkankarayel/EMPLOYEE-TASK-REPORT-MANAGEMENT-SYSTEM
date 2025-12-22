using EmployeeTaskManagement.Application.DTOs.LeaveRequest;
using FluentValidation;

namespace EmployeeTaskManagement.Application.Validators;

public class UpdateLeaveRequestValidator : AbstractValidator<UpdateLeaveRequestDto>
{
    public UpdateLeaveRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Geçerli bir izin talebi ID giriniz.");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("İzin durumu zorunludur.")
            .Must(status => status == "Pending" || status == "Approved" || status == "Rejected")
            .WithMessage("İzin durumu 'Pending', 'Approved' veya 'Rejected' olmalıdır.");
    }
}
