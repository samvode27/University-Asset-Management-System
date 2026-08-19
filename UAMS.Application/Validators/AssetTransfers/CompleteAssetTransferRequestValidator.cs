using FluentValidation;
using UAMS.Application.DTOs.AssetTransfers.Requests;

namespace UAMS.Application.Validators.AssetTransfers;

public class CompleteAssetTransferRequestValidator
    : AbstractValidator<CompleteAssetTransferRequestDto>
{
    public CompleteAssetTransferRequestValidator()
    {
        RuleFor(x => x.Remarks)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.Remarks))
            .WithMessage(
                "Remarks must not exceed 1000 characters.");
    }
}