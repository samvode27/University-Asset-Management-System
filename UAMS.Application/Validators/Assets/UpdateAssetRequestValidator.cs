using FluentValidation;
using UAMS.Application.DTOs.Assets.Requests;

namespace UAMS.Application.Validators.Assets;

public class UpdateAssetRequestValidator
    : AbstractValidator<UpdateAssetRequestDto>
{
    public UpdateAssetRequestValidator()
    {
        // ============================================================
        // Name
        // ============================================================

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Asset name is required.")
            .MaximumLength(200)
            .WithMessage("Asset name must not exceed 200 characters.");


        // ============================================================
        // Description
        // ============================================================

        RuleFor(x => x.Description)
            .MaximumLength(2000)
            .WithMessage("Description must not exceed 2000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));


        // ============================================================
        // Serial Number
        // ============================================================

        RuleFor(x => x.SerialNumber)
            .MaximumLength(100)
            .WithMessage("Serial number must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.SerialNumber));


        // ============================================================
        // Model
        // ============================================================

        RuleFor(x => x.Model)
            .MaximumLength(150)
            .WithMessage("Model must not exceed 150 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Model));


        // ============================================================
        // Manufacturer
        // ============================================================

        RuleFor(x => x.Manufacturer)
            .MaximumLength(150)
            .WithMessage("Manufacturer must not exceed 150 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Manufacturer));


        // ============================================================
        // Asset Category
        // ============================================================

        RuleFor(x => x.AssetCategoryId)
            .NotEmpty()
            .WithMessage("Asset category is required.");


        // ============================================================
        // Purchase Cost
        // ============================================================

        RuleFor(x => x.PurchaseCost)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Purchase cost cannot be negative.");


        // ============================================================
        // Purchase Date
        // ============================================================

        RuleFor(x => x.PurchaseDate)
            .NotEmpty()
            .WithMessage("Purchase date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Purchase date cannot be in the future.");


        // ============================================================
        // Warranty Expiry
        // ============================================================

        RuleFor(x => x.WarrantyExpiryDate)
            .GreaterThanOrEqualTo(x => x.PurchaseDate)
            .When(x => x.WarrantyExpiryDate.HasValue)
            .WithMessage("Warranty expiry date cannot be earlier than the purchase date.");


        // ============================================================
        // Location
        // ============================================================

        RuleFor(x => x.Location)
            .MaximumLength(500)
            .WithMessage("Location must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Location));
    }
}