using EmployeeTaskManagement.Application.DTOs.Task;
using FluentValidation;

namespace EmployeeTaskManagement.Application.Validators;

public class UpdateTaskValidator : AbstractValidator<UpdateTaskDto>
{
    public UpdateTaskValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Geçerli bir görev ID giriniz.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Görev başlığı zorunludur.")
            .MinimumLength(3).WithMessage("Görev başlığı en az 3 karakter olmalıdır.")
            .MaximumLength(200).WithMessage("Görev başlığı en fazla 200 karakter olabilir.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Görev açıklaması zorunludur.")
            .MinimumLength(10).WithMessage("Görev açıklaması en az 10 karakter olmalıdır.")
            .MaximumLength(1000).WithMessage("Görev açıklaması en fazla 1000 karakter olabilir.");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Görev durumu zorunludur.")
            .Must(status => status == "Pending" || status == "Started" || status == "Completed")
            .WithMessage("Görev durumu 'Pending', 'Started' veya 'Completed' olmalıdır.");

        RuleFor(x => x.DueDate)
            .NotEmpty().WithMessage("Bitiş tarihi zorunludur.");

        RuleFor(x => x.AssignedUserId)
            .GreaterThan(0).WithMessage("Geçerli bir kullanıcı seçiniz.");
    }
}
