using FluentValidation;
using UAMS.Application.DTOs.AssetRequests.Requests;

namespace UAMS.Application.Validators.AssetRequests;

public class CancelAssetRequestValidator
    : AbstractValidator<CancelAssetRequestDto>
{
    public CancelAssetRequestValidator()
    {
        // ============================================================
        // Cancellation Reason
        // ============================================================

        RuleFor(x => x.Reason)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.Reason))
            .WithMessage(
                "Cancellation reason must not exceed 1000 characters.");
    }
}