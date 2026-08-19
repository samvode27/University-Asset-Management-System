using FluentValidation;
using UAMS.Application.DTOs.DamageReports.Requests;

namespace UAMS.Application.Validators.DamageReports;

public class RejectDamageReportRequestValidator
    : AbstractValidator<RejectDamageReportRequestDto>
{
    public RejectDamageReportRequestValidator()
    {
        RuleFor(x => x.RejectionReason)
            .NotEmpty()
            .WithMessage("Rejection reason is required.")
            .MaximumLength(2000)
            .WithMessage("Rejection reason cannot exceed 2000 characters.");
    }
}