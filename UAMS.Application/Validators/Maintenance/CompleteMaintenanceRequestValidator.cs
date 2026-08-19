using FluentValidation;
using UAMS.Application.DTOs.Maintenance.Requests;

namespace UAMS.Application.Validators.Maintenance;

public class CompleteMaintenanceRequestValidator
    : AbstractValidator<CompleteMaintenanceRequestDto>
{
    public CompleteMaintenanceRequestValidator()
    {
        RuleFor(x => x.Result)
            .IsInEnum()
            .WithMessage("Invalid maintenance result.");

        RuleFor(x => x.ActualCost)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Actual cost cannot be negative.");

        RuleFor(x => x.MaintenanceDescription)
            .MaximumLength(2000)
            .When(x => !string.IsNullOrWhiteSpace(x.MaintenanceDescription))
            .WithMessage(
                "Maintenance description cannot exceed 2000 characters.");

        RuleFor(x => x.PartsUsed)
            .MaximumLength(2000)
            .When(x => !string.IsNullOrWhiteSpace(x.PartsUsed))
            .WithMessage("Parts used cannot exceed 2000 characters.");

        RuleFor(x => x.FailureReason)
            .MaximumLength(2000)
            .When(x => !string.IsNullOrWhiteSpace(x.FailureReason))
            .WithMessage("Failure reason cannot exceed 2000 characters.");

        RuleFor(x => x.Remarks)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.Remarks))
            .WithMessage("Remarks cannot exceed 1000 characters.");
    }
}