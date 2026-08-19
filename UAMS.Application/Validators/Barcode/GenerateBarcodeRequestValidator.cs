using FluentValidation;
using UAMS.Application.DTOs.Barcode.Requests;

namespace UAMS.Application.Validators.Barcode;

public class GenerateBarcodeRequestValidator
    : AbstractValidator<GenerateBarcodeRequestDto>
{
    public GenerateBarcodeRequestValidator()
    {
        // ============================================================
        // Asset
        // ============================================================

        RuleFor(x => x.AssetId)
            .NotEmpty()
            .WithMessage("Asset is required.");


        // ============================================================
        // Barcode Format
        // ============================================================

        RuleFor(x => x.Format)
            .IsInEnum()
            .WithMessage("Invalid barcode format.");


        // ============================================================
        // Expiration Date
        // ============================================================

        RuleFor(x => x.ExpiresAt)
            .GreaterThan(DateTime.UtcNow)
            .When(x => x.ExpiresAt.HasValue)
            .WithMessage("Barcode expiration date must be in the future.");
    }
}