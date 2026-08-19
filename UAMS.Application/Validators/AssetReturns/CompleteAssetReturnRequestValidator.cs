using FluentValidation;
using UAMS.Application.DTOs.AssetReturns.Requests;

namespace UAMS.Application.Validators.AssetReturns;

public class CompleteAssetReturnRequestValidator
    : AbstractValidator<CompleteAssetReturnRequestDto>
{
    public CompleteAssetReturnRequestValidator()
    {
        RuleFor(x => x.Remarks)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.Remarks))
            .WithMessage("Remarks cannot exceed 1000 characters.");
    }
}