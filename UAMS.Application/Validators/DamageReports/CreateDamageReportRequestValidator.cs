using FluentValidation;
using UAMS.Application.DTOs.DamageReports.Requests;

namespace UAMS.Application.Validators.DamageReports;

public class CreateDamageReportRequestValidator
    : AbstractValidator<CreateDamageReportRequestDto>
{
    public CreateDamageReportRequestValidator()
    {
        RuleFor(x => x.AssetId)
            .NotEmpty()
            .WithMessage("Asset is required.");

        RuleFor(x => x.AssetAssignmentId)
            .NotEmpty()
            .WithMessage("Asset assignment is required.");

        RuleFor(x => x.DamageType)
            .IsInEnum()
            .WithMessage("Invalid damage type.");

        RuleFor(x => x.Severity)
            .IsInEnum()
            .WithMessage("Invalid damage severity.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Damage description is required.")
            .MaximumLength(2000)
            .WithMessage("Damage description cannot exceed 2000 characters.");

        RuleFor(x => x.IncidentDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .When(x => x.IncidentDate.HasValue)
            .WithMessage("Incident date cannot be in the future.");

        RuleFor(x => x.IncidentLocation)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.IncidentLocation))
            .WithMessage("Incident location cannot exceed 500 characters.");

        RuleFor(x => x.Remarks)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.Remarks))
            .WithMessage("Remarks cannot exceed 1000 characters.");
    }
}