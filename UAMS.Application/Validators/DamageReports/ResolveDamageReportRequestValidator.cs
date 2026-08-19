using FluentValidation;
using UAMS.Application.DTOs.DamageReports.Requests;

namespace UAMS.Application.Validators.DamageReports;

public class ResolveDamageReportRequestValidator
    : AbstractValidator<ResolveDamageReportRequestDto>
{
    public ResolveDamageReportRequestValidator()
    {
        RuleFor(x => x.ResolutionRemarks)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.ResolutionRemarks))
            .WithMessage("Resolution remarks cannot exceed 1000 characters.");
    }
}