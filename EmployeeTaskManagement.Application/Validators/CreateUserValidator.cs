using EmployeeTaskManagement.Application.DTOs.User;
using FluentValidation;

namespace EmployeeTaskManagement.Application.Validators;

public class CreateUserValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Ad Soyad alanı zorunludur.")
            .MinimumLength(3).WithMessage("Ad Soyad en az 3 karakter olmalıdır.")
            .MaximumLength(100).WithMessage("Ad Soyad en fazla 100 karakter olabilir.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email alanı zorunludur.")
            .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.")
            .MaximumLength(150).WithMessage("Email en fazla 150 karakter olabilir.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre alanı zorunludur.")
            .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.")
            .MaximumLength(50).WithMessage("Şifre en fazla 50 karakter olabilir.");

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Rol alanı zorunludur.")
            .Must(role => role == "Admin" || role == "User")
            .WithMessage("Rol sadece 'Admin' veya 'User' olabilir.");
    }
}
