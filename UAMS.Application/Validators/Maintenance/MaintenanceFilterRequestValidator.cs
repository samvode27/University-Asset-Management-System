using FluentValidation;
using UAMS.Application.DTOs.Maintenance.Requests;

namespace UAMS.Application.Validators.Maintenance;

public class MaintenanceFilterRequestValidator
    : AbstractValidator<MaintenanceFilterRequestDto>
{
    public MaintenanceFilterRequestValidator()
    {
        RuleFor(x => x.MaintenanceNumber)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.MaintenanceNumber))
            .WithMessage("Maintenance number cannot exceed 100 characters.");

        RuleFor(x => x.MaintenanceType)
            .IsInEnum()
            .When(x => x.MaintenanceType.HasValue)
            .WithMessage("Invalid maintenance type.");

        RuleFor(x => x.Status)
            .IsInEnum()
            .When(x => x.Status.HasValue)
            .WithMessage("Invalid maintenance status.");

        RuleFor(x => x.Result)
            .IsInEnum()
            .When(x => x.Result.HasValue)
            .WithMessage("Invalid maintenance result.");

        RuleFor(x => x.RequestedFromDate)
            .LessThanOrEqualTo(x => x.RequestedToDate)
            .When(x => x.RequestedFromDate.HasValue &&
                       x.RequestedToDate.HasValue)
            .WithMessage(
                "Requested from date must be before or equal to requested to date.");

        RuleFor(x => x.CompletedFromDate)
            .LessThanOrEqualTo(x => x.CompletedToDate)
            .When(x => x.CompletedFromDate.HasValue &&
                       x.CompletedToDate.HasValue)
            .WithMessage(
                "Completed from date must be before or equal to completed to date.");

        RuleFor(x => x.RequestedToDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .When(x => x.RequestedToDate.HasValue)
            .WithMessage("Requested to date cannot be in the future.");

        RuleFor(x => x.CompletedToDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .When(x => x.CompletedToDate.HasValue)
            .WithMessage("Completed to date cannot be in the future.");

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100.");

        RuleFor(x => x.SearchTerm)
            .MaximumLength(200)
            .When(x => !string.IsNullOrWhiteSpace(x.SearchTerm))
            .WithMessage("Search term cannot exceed 200 characters.");
    }
}