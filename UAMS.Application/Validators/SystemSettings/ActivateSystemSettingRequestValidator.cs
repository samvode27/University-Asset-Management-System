using FluentValidation;
using UAMS.Application.DTOs.SystemSettings.Requests;

namespace UAMS.Application.Validators.SystemSettings;

public class ActivateSystemSettingRequestValidator
    : AbstractValidator<ActivateSystemSettingRequestDto>
{
    public ActivateSystemSettingRequestValidator()
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