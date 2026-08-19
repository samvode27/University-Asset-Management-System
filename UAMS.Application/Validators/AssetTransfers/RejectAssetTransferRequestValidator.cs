using FluentValidation;
using UAMS.Application.DTOs.AssetTransfers.Requests;

namespace UAMS.Application.Validators.AssetTransfers;

public class RejectAssetTransferRequestValidator
    : AbstractValidator<RejectAssetTransferRequestDto>
{
    public RejectAssetTransferRequestValidator()
    {
        RuleFor(x => x.ApprovalRemarks)
            .NotEmpty()
            .WithMessage(
                "Approval remarks are required when rejecting an asset transfer.")
            .MaximumLength(1000)
            .WithMessage(
                "Approval remarks must not exceed 1000 characters.");
    }
}