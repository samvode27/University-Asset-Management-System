using FluentValidation;
using UAMS.Application.DTOs.SystemSettings.Requests;

namespace UAMS.Application.Validators.SystemSettings;

public class DeactivateSystemSettingRequestValidator
    : AbstractValidator<DeactivateSystemSettingRequestDto>
{
    public DeactivateSystemSettingRequestValidator()
    {
        // ============================================================
        // Remarks
        // ============================================================

        RuleFor(x => x.Remarks)
            .MaximumLength(1000)
            .WithMessage("Remarks must not exceed 1000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Remarks));
    }
}