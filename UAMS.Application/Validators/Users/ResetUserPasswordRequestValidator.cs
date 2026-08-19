using FluentValidation;
using UAMS.Application.DTOs.Users.Requests;

namespace UAMS.Application.Validators.Users;

public class ResetUserPasswordRequestValidator
    : AbstractValidator<ResetUserPasswordRequestDto>
{
    public ResetUserPasswordRequestValidator()
    {
        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage("New password is required.")
            .MinimumLength(8)
            .WithMessage("New password must be at least 8 characters long.")
            .MaximumLength(100)
            .WithMessage("New password cannot exceed 100 characters.")
            .Matches("[A-Z]")
            .WithMessage("New password must contain at least one uppercase letter.")
            .Matches("[a-z]")
            .WithMessage("New password must contain at least one lowercase letter.")
            .Matches("[0-9]")
            .WithMessage("New password must contain at least one number.")
            .Matches("[^a-zA-Z0-9]")
            .WithMessage("New password must contain at least one special character.");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .WithMessage("Password confirmation is required.")
            .Equal(x => x.NewPassword)
            .WithMessage("Password confirmation does not match the new password.");
    }
}