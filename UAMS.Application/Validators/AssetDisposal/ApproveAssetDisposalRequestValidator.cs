using FluentValidation;
using UAMS.Application.DTOs.AssetDisposals.Requests;

namespace UAMS.Application.Validators.AssetDisposals;

public class ApproveAssetDisposalRequestValidator
    : AbstractValidator<ApproveAssetDisposalRequestDto>
{
    public ApproveAssetDisposalRequestValidator()
    {
        RuleFor(x => x.DisposalMethod)
            .IsInEnum()
            .WithMessage("A valid disposal method is required.");

        RuleFor(x => x.Remarks)
            .MaximumLength(2000)
            .When(x => !string.IsNullOrWhiteSpace(x.Remarks))
            .WithMessage("Remarks must not exceed 2000 characters.");
    }
}