using FluentValidation;
using UAMS.Application.DTOs.AssetAssignments.Requests;

namespace UAMS.Application.Validators.AssetAssignments;

public class CompleteAssetAssignmentRequestValidator
    : AbstractValidator<CompleteAssetAssignmentRequestDto>
{
    public CompleteAssetAssignmentRequestValidator()
    {
        // ============================================================
        // Actual Return Date
        // ============================================================

        RuleFor(x => x.ActualReturnDate)
            .NotEmpty()
            .WithMessage("Actual return date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage(
                "Actual return date cannot be in the future.");


        // ============================================================
        // Condition At Return
        // ============================================================

        RuleFor(x => x.ConditionAtReturn)
            .IsInEnum()
            .WithMessage("Invalid asset condition at return.");


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