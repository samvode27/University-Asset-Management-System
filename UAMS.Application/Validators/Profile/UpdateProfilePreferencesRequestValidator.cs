using FluentValidation;
using UAMS.Application.DTOs.Profile.Requests;

namespace UAMS.Application.Validators.Profile;

public class UpdateProfilePreferencesRequestValidator
    : AbstractValidator<UpdateProfilePreferencesRequestDto>
{
    public UpdateProfilePreferencesRequestValidator()
    {
        // ============================================================
        // Language
        // ============================================================

        RuleFor(x => x.Language)
            .MaximumLength(20)
            .WithMessage("Language cannot exceed 20 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Language));


        // ============================================================
        // Time Zone
        // ============================================================

        RuleFor(x => x.TimeZone)
            .MaximumLength(100)
            .WithMessage("Time zone cannot exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.TimeZone));
    }
}