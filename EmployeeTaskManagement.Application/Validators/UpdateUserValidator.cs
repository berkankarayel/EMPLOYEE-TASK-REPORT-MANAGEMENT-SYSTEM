using EmployeeTaskManagement.Application.DTOs.User;
using FluentValidation;

namespace EmployeeTaskManagement.Application.Validators;

public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Geçerli bir kullanıcı ID giriniz.");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Ad Soyad alanı zorunludur.")
            .MinimumLength(3).WithMessage("Ad Soyad en az 3 karakter olmalıdır.")
            .MaximumLength(100).WithMessage("Ad Soyad en fazla 100 karakter olabilir.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email alanı zorunludur.")
            .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.")
            .MaximumLength(150).WithMessage("Email en fazla 150 karakter olabilir.");

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Rol alanı zorunludur.")
            .Must(role => role == "Admin" || role == "User")
            .WithMessage("Rol sadece 'Admin' veya 'User' olabilir.");
    }
}
