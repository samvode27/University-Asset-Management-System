using FluentValidation;
using UAMS.Application.DTOs.AssetAssignments.Requests;

namespace UAMS.Application.Validators.AssetAssignments;

public class AssetAssignmentFilterRequestValidator
    : AbstractValidator<AssetAssignmentFilterRequestDto>
{
    public AssetAssignmentFilterRequestValidator()
    {
        // ============================================================
        // Status
        // ============================================================

        RuleFor(x => x.Status)
            .IsInEnum()
            .When(x => x.Status.HasValue)
            .WithMessage("Invalid asset assignment status.");


        // ============================================================
        // Assigned Date Range
        // ============================================================

        RuleFor(x => x.AssignedDateTo)
            .GreaterThanOrEqualTo(x => x.AssignedDateFrom)
            .When(x =>
                x.AssignedDateFrom.HasValue &&
                x.AssignedDateTo.HasValue)
            .WithMessage(
                "Assigned date to cannot be earlier than assigned date from.");


        // ============================================================
        // Expected Return Date Range
        // ============================================================

        RuleFor(x => x.ExpectedReturnDateTo)
            .GreaterThanOrEqualTo(x => x.ExpectedReturnDateFrom)
            .When(x =>
                x.ExpectedReturnDateFrom.HasValue &&
                x.ExpectedReturnDateTo.HasValue)
            .WithMessage(
                "Expected return date to cannot be earlier than expected return date from.");


        // ============================================================
        // Search Term
        // ============================================================

        RuleFor(x => x.SearchTerm)
            .MaximumLength(200)
            .When(x => !string.IsNullOrWhiteSpace(x.SearchTerm))
            .WithMessage(
                "Search term must not exceed 200 characters.");


        // ============================================================
        // Page Number
        // ============================================================

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage(
                "Page number must be greater than or equal to 1.");


        // ============================================================
        // Page Size
        // ============================================================

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage(
                "Page size must be between 1 and 100.");
    }
}