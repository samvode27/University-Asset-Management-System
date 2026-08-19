using FluentValidation;
using UAMS.Application.DTOs.Authentication.Requests;

namespace UAMS.Application.Validators.Authentication;

public class LoginRequestValidator
    : AbstractValidator<LoginRequestDto>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.UsernameOrEmail)
            .NotEmpty()
            .WithMessage("Username or email is required.")
            .MaximumLength(255)
            .WithMessage("Username or email cannot exceed 255 characters.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MaximumLength(100)
            .WithMessage("Password cannot exceed 100 characters.");
    }
}