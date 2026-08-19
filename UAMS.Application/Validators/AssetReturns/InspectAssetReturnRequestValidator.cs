using FluentValidation;
using UAMS.Application.DTOs.AssetReturns.Requests;

namespace UAMS.Application.Validators.AssetReturns;

public class InspectAssetReturnRequestValidator
    : AbstractValidator<InspectAssetReturnRequestDto>
{
    public InspectAssetReturnRequestValidator()
    {
        RuleFor(x => x.InspectedById)
            .NotEmpty()
            .WithMessage("Inspector is required.");

        RuleFor(x => x.InspectionDate)
            .NotEmpty()
            .WithMessage("Inspection date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Inspection date cannot be in the future.");

        RuleFor(x => x.InspectionNotes)
            .MaximumLength(2000)
            .When(x => !string.IsNullOrWhiteSpace(x.InspectionNotes))
            .WithMessage("Inspection notes cannot exceed 2000 characters.");

        RuleFor(x => x.DamageReportId)
            .Must(id => id == null || id != Guid.Empty)
            .WithMessage("Damage report ID must be valid when provided.");

        RuleFor(x => x.Remarks)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.Remarks))
            .WithMessage("Remarks cannot exceed 1000 characters.");

        RuleFor(x => x.DamageReportId)
            .NotEmpty()
            .When(x => x.DamageFound)
            .WithMessage(
                "A damage report is required when damage is found.");
    }
}