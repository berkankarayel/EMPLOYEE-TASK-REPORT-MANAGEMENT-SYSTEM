using EmployeeTaskManagement.Application.DTOs.LeaveRequest;
using FluentValidation;

namespace EmployeeTaskManagement.Application.Validators;

public class CreateLeaveRequestValidator : AbstractValidator<CreateLeaveRequestDto>
{
    public CreateLeaveRequestValidator()
    {
        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Başlangıç tarihi zorunludur.")
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date).WithMessage("Başlangıç tarihi bugünden önceki bir tarih olamaz.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("Bitiş tarihi zorunludur.")
            .GreaterThan(x => x.StartDate).WithMessage("Bitiş tarihi başlangıç tarihinden sonra olmalıdır.");

        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("İzin sebebi zorunludur.")
            .MinimumLength(10).WithMessage("İzin sebebi en az 10 karakter olmalıdır.")
            .MaximumLength(500).WithMessage("İzin sebebi en fazla 500 karakter olabilir.");
    }
}
