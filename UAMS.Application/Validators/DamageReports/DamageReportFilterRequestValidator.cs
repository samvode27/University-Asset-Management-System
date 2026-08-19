using FluentValidation;
using UAMS.Application.DTOs.DamageReports.Requests;

namespace UAMS.Application.Validators.DamageReports;

public class DamageReportFilterRequestValidator
    : AbstractValidator<DamageReportFilterRequestDto>
{
    public DamageReportFilterRequestValidator()
    {
        RuleFor(x => x.ReportNumber)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.ReportNumber))
            .WithMessage("Report number cannot exceed 100 characters.");

        RuleFor(x => x.ReportedFromDate)
            .LessThanOrEqualTo(x => x.ReportedToDate)
            .When(x => x.ReportedFromDate.HasValue &&
                       x.ReportedToDate.HasValue)
            .WithMessage("Reported from date must be before or equal to reported to date.");

        RuleFor(x => x.ReportedToDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .When(x => x.ReportedToDate.HasValue)
            .WithMessage("Reported to date cannot be in the future.");

        RuleFor(x => x.DamageType)
            .IsInEnum()
            .When(x => x.DamageType.HasValue)
            .WithMessage("Invalid damage type.");

        RuleFor(x => x.Severity)
            .IsInEnum()
            .When(x => x.Severity.HasValue)
            .WithMessage("Invalid damage severity.");

        RuleFor(x => x.Status)
            .IsInEnum()
            .When(x => x.Status.HasValue)
            .WithMessage("Invalid damage report status.");

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