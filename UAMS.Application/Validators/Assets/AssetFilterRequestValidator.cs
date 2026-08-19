using FluentValidation;
using UAMS.Application.DTOs.Assets.Requests;

namespace UAMS.Application.Validators.Assets;

public class AssetFilterRequestValidator
    : AbstractValidator<AssetFilterRequestDto>
{
    public AssetFilterRequestValidator()
    {
        // ============================================================
        // Asset Tag
        // ============================================================

        RuleFor(x => x.AssetTag)
            .MaximumLength(100)
            .WithMessage("Asset tag must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.AssetTag));


        // ============================================================
        // Name
        // ============================================================

        RuleFor(x => x.Name)
            .MaximumLength(200)
            .WithMessage("Asset name must not exceed 200 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Name));


        // ============================================================
        // Serial Number
        // ============================================================

        RuleFor(x => x.SerialNumber)
            .MaximumLength(100)
            .WithMessage("Serial number must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.SerialNumber));


        // ============================================================
        // Manufacturer
        // ============================================================

        RuleFor(x => x.Manufacturer)
            .MaximumLength(150)
            .WithMessage("Manufacturer must not exceed 150 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Manufacturer));


        // ============================================================
        // Model
        // ============================================================

        RuleFor(x => x.Model)
            .MaximumLength(150)
            .WithMessage("Model must not exceed 150 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Model));


        // ============================================================
        // Location
        // ============================================================

        RuleFor(x => x.Location)
            .MaximumLength(500)
            .WithMessage("Location must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Location));


        // ============================================================
        // Purchase Date Range
        // ============================================================

        RuleFor(x => x.PurchaseDateTo)
            .GreaterThanOrEqualTo(x => x.PurchaseDateFrom)
            .When(x =>
                x.PurchaseDateFrom.HasValue &&
                x.PurchaseDateTo.HasValue)
            .WithMessage(
                "Purchase date to cannot be earlier than purchase date from.");


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
            .WithMessage("Sort property must not exceed 50 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.SortBy));
    }
}