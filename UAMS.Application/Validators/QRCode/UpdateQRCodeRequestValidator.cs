using FluentValidation;
using UAMS.Application.DTOs.QRCode.Requests;

namespace UAMS.Application.Validators.QRCode;

public class UpdateQRCodeRequestValidator
    : AbstractValidator<UpdateQRCodeRequestDto>
{
    public UpdateQRCodeRequestValidator()
    {
        // ============================================================
        // Expiration Date
        // ============================================================

        RuleFor(x => x.ExpiresAt)
            .GreaterThan(DateTime.UtcNow)
            .When(x => x.ExpiresAt.HasValue)
            .WithMessage("QR code expiration date must be in the future.");
    }
}