using FluentValidation;
using UAMS.Application.DTOs.AssetRequests.Requests;

namespace UAMS.Application.Validators.AssetRequests;

public class AssetRequestFilterRequestValidator
    : AbstractValidator<AssetRequestFilterRequestDto>
{
    public AssetRequestFilterRequestValidator()
    {
        // ============================================================
        // Request Number
        // ============================================================

        RuleFor(x => x.RequestNumber)
            .MaximumLength(50)
            .When(x => !string.IsNullOrWhiteSpace(x.RequestNumber))
            .WithMessage(
                "Request number must not exceed 50 characters.");


        // ============================================================
        // Requested Date Range
        // ============================================================

        RuleFor(x => x.RequestedTo)
            .GreaterThanOrEqualTo(x => x.RequestedFrom)
            .When(x =>
                x.RequestedFrom.HasValue &&
                x.RequestedTo.HasValue)
            .WithMessage(
                "Requested to date cannot be earlier than requested from date.");


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


        // ============================================================
        // Sort By
        // ============================================================

        RuleFor(x => x.SortBy)
            .MaximumLength(50)
            .When(x => !string.IsNullOrWhiteSpace(x.SortBy))
            .WithMessage(
                "Sort property must not exceed 50 characters.");
    }
}