using FluentValidation;
using UAMS.Application.DTOs.Profile.Requests;

namespace UAMS.Application.Validators.Profile;

public class UpdateProfileRequestValidator
    : AbstractValidator<UpdateProfileRequestDto>
{
    public UpdateProfileRequestValidator()
    {
        // ============================================================
        // Full Name
        // ============================================================

        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("Full name is required.")
            .MaximumLength(200)
            .WithMessage("Full name cannot exceed 200 characters.");


        // ============================================================
        // Email
        // ============================================================

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .MaximumLength(255)
            .WithMessage("Email cannot exceed 255 characters.")
            .EmailAddress()
            .WithMessage("A valid email address is required.");


        // ============================================================
        // Phone Number
        // ============================================================

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required.")
            .MaximumLength(30)
            .WithMessage("Phone number cannot exceed 30 characters.");
    }
}