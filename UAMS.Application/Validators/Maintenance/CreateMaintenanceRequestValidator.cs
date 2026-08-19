using FluentValidation;
using UAMS.Application.DTOs.Maintenance.Requests;

namespace UAMS.Application.Validators.Maintenance;

public class CreateMaintenanceRequestValidator
    : AbstractValidator<CreateMaintenanceRequestDto>
{
    public CreateMaintenanceRequestValidator()
    {
        RuleFor(x => x.AssetId)
            .NotEmpty()
            .WithMessage("Asset is required.");

        RuleFor(x => x.DamageReportId)
            .Must(id => id == null || id != Guid.Empty)
            .WithMessage("Damage report ID must be valid when provided.");

        RuleFor(x => x.RequestedById)
            .NotEmpty()
            .WithMessage("Requested by user is required.");

        RuleFor(x => x.MaintenanceType)
            .IsInEnum()
            .WithMessage("Invalid maintenance type.");

        RuleFor(x => x.ProblemDescription)
            .NotEmpty()
            .WithMessage("Problem description is required.")
            .MaximumLength(2000)
            .WithMessage("Problem description cannot exceed 2000 characters.");

        RuleFor(x => x.MaintenanceDescription)
            .MaximumLength(2000)
            .When(x => !string.IsNullOrWhiteSpace(x.MaintenanceDescription))
            .WithMessage("Maintenance description cannot exceed 2000 characters.");

        RuleFor(x => x.PartsUsed)
            .MaximumLength(2000)
            .When(x => !string.IsNullOrWhiteSpace(x.PartsUsed))
            .WithMessage("Parts used cannot exceed 2000 characters.");

        RuleFor(x => x.EstimatedCost)
            .GreaterThanOrEqualTo(0)
            .When(x => x.EstimatedCost.HasValue)
            .WithMessage("Estimated cost cannot be negative.");

        RuleFor(x => x.RequestedDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .When(x => x.RequestedDate.HasValue)
            .WithMessage("Requested date cannot be in the future.");

        RuleFor(x => x.Remarks)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.Remarks))
            .WithMessage("Remarks cannot exceed 1000 characters.");
    }
}