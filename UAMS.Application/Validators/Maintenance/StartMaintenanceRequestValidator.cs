using FluentValidation;
using UAMS.Application.DTOs.Maintenance.Requests;

namespace UAMS.Application.Validators.Maintenance;

public class StartMaintenanceRequestValidator
    : AbstractValidator<StartMaintenanceRequestDto>
{
    public StartMaintenanceRequestValidator()
    {
        RuleFor(x => x.MaintenanceDescription)
            .MaximumLength(2000)
            .When(x => !string.IsNullOrWhiteSpace(x.MaintenanceDescription))
            .WithMessage(
                "Maintenance description cannot exceed 2000 characters.");

        RuleFor(x => x.PartsUsed)
            .MaximumLength(2000)
            .When(x => !string.IsNullOrWhiteSpace(x.PartsUsed))
            .WithMessage("Parts used cannot exceed 2000 characters.");

        RuleFor(x => x.Remarks)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.Remarks))
            .WithMessage("Remarks cannot exceed 1000 characters.");
    }
}