using FluentValidation;
using LibrariesManagementSystem.Api.DTOs.Auth;

namespace LibrariesManagementSystem.Api.Validators;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email обязателен.")
            .EmailAddress().WithMessage("Некорректный формат email.");
        
        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Пароль обязателен.")
            .MinimumLength(6).WithMessage("Пароль должен содержать не менее 6 символов.");
    }
}