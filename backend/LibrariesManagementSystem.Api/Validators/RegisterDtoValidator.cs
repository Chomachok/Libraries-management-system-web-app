using FluentValidation;
using LibrariesManagementSystem.Api.DTOs.Auth;

namespace LibrariesManagementSystem.Api.Validators;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(user => user.FullName)
            .NotEmpty().WithMessage("ФИО обязательно.")
            .MinimumLength(2).WithMessage("Имя должно содержать хотя бы 2 символа.");

        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email обязателен.")
            .EmailAddress().WithMessage("Некорректный формат email.");

        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Пароль обязателен.")
            .MinimumLength(6).WithMessage("Пароль должен содержать не менее 6 символов.")
            .Matches("[A-Z]").WithMessage("Пароль должен содержать хотя бы одну заглавную букву.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Пароль должен содержать хотя бы один специальный символ.");

        RuleFor(user => user.LibraryId)
            .GreaterThan(0).WithMessage("Выберите библиотеку.");
    }
}