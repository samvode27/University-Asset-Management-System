using FluentValidation;
using UAMS.Application.DTOs.Barcode.Requests;

namespace UAMS.Application.Validators.Barcode;

public class BarcodeFilterRequestValidator
    : AbstractValidator<BarcodeFilterRequestDto>
{
    public BarcodeFilterRequestValidator()
    {
        // ============================================================
        // Barcode Code
        // ============================================================

        RuleFor(x => x.Code)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Code))
            .WithMessage("Barcode code must not exceed 500 characters.");


        // ============================================================
        // Barcode Format
        // ============================================================

        RuleFor(x => x.Format)
            .IsInEnum()
            .When(x => x.Format.HasValue)
            .WithMessage("Invalid barcode format.");


        // ============================================================
        // Generated Date Range
        // ============================================================

        RuleFor(x => x.GeneratedTo)
            .GreaterThanOrEqualTo(x => x.GeneratedFrom)
            .When(x =>
                x.GeneratedFrom.HasValue &&
                x.GeneratedTo.HasValue)
            .WithMessage(
                "Generated to date cannot be earlier than generated from date.");


        // ============================================================
        // Expiration Date Range
        // ============================================================

        RuleFor(x => x.ExpiresTo)
            .GreaterThanOrEqualTo(x => x.ExpiresFrom)
            .When(x =>
                x.ExpiresFrom.HasValue &&
                x.ExpiresTo.HasValue)
            .WithMessage(
                "Expires to date cannot be earlier than expires from date.");


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