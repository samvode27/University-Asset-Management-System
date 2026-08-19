using FluentValidation;
using UAMS.Application.DTOs.Maintenance.Requests;

namespace UAMS.Application.Validators.Maintenance;

public class CancelMaintenanceRequestValidator
    : AbstractValidator<CancelMaintenanceRequestDto>
{
    public CancelMaintenanceRequestValidator()
    {
        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Cancellation reason is required.")
            .MaximumLength(1000)
            .WithMessage("Cancellation reason cannot exceed 1000 characters.");
    }
}