using FluentValidation;
using UAMS.Application.DTOs.Authentication.Requests;

namespace UAMS.Application.Validators.Authentication;

public class VerifyEmailRequestValidator
    : AbstractValidator<VerifyEmailRequestDto>
{
    public VerifyEmailRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("A valid email address is required.")
            .MaximumLength(255)
            .WithMessage("Email cannot exceed 255 characters.");

        RuleFor(x => x.Token)
            .NotEmpty()
            .WithMessage("Verification token is required.");
    }
}