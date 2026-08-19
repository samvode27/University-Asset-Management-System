using FluentValidation;
using UAMS.Application.DTOs.Barcode.Requests;

namespace UAMS.Application.Validators.Barcode;

public class UpdateBarcodeRequestValidator
    : AbstractValidator<UpdateBarcodeRequestDto>
{
    public UpdateBarcodeRequestValidator()
    {
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