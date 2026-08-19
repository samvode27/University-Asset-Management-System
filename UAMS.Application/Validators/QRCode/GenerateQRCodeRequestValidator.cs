using FluentValidation;
using UAMS.Application.DTOs.QRCode.Requests;

namespace UAMS.Application.Validators.QRCode;

public class GenerateQRCodeRequestValidator
    : AbstractValidator<GenerateQRCodeRequestDto>
{
    public GenerateQRCodeRequestValidator()
    {
        // ============================================================
        // Asset
        // ============================================================

        RuleFor(x => x.AssetId)
            .NotEmpty()
            .WithMessage("Asset is required.");


        // ============================================================
        // Expiration Date
        // ============================================================

        RuleFor(x => x.ExpiresAt)
            .GreaterThan(DateTime.UtcNow)
            .When(x => x.ExpiresAt.HasValue)
            .WithMessage("QR code expiration date must be in the future.");
    }
}