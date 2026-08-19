using FluentValidation;
using UAMS.Application.DTOs.AssetReturns.Requests;

namespace UAMS.Application.Validators.AssetReturns;

public class CancelAssetReturnRequestValidator
    : AbstractValidator<CancelAssetReturnRequestDto>
{
    public CancelAssetReturnRequestValidator()
    {
        RuleFor(x => x.Reason)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.Reason))
            .WithMessage("Cancellation reason cannot exceed 1000 characters.");
    }
}