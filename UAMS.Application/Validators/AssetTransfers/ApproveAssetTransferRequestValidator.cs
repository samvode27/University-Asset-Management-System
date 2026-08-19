using FluentValidation;
using UAMS.Application.DTOs.AssetTransfers.Requests;

namespace UAMS.Application.Validators.AssetTransfers;

public class ApproveAssetTransferRequestValidator
    : AbstractValidator<ApproveAssetTransferRequestDto>
{
    public ApproveAssetTransferRequestValidator()
    {
        RuleFor(x => x.ApprovalRemarks)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.ApprovalRemarks))
            .WithMessage(
                "Approval remarks must not exceed 1000 characters.");
    }
}