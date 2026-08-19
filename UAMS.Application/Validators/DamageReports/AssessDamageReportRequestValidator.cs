using FluentValidation;
using UAMS.Application.DTOs.DamageReports.Requests;

namespace UAMS.Application.Validators.DamageReports;

public class AssessDamageReportRequestValidator
    : AbstractValidator<AssessDamageReportRequestDto>
{
    public AssessDamageReportRequestValidator()
    {
        RuleFor(x => x.Assessment)
            .NotEmpty()
            .WithMessage("Assessment is required.")
            .MaximumLength(2000)
            .WithMessage("Assessment cannot exceed 2000 characters.");
    }
}