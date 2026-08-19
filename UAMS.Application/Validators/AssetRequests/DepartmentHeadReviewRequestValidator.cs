using FluentValidation;
using UAMS.Application.DTOs.AssetRequests.Requests;

namespace UAMS.Application.Validators.AssetRequests;

public class DepartmentHeadReviewRequestValidator
    : AbstractValidator<DepartmentHeadReviewRequestDto>
{
    public DepartmentHeadReviewRequestValidator()
    {
        // ============================================================
        // Remarks
        // ============================================================

        RuleFor(x => x.Remarks)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.Remarks))
            .WithMessage(
                "Remarks must not exceed 1000 characters.");
    }
}