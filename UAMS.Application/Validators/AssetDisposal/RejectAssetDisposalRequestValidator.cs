using FluentValidation;
using UAMS.Application.DTOs.AssetDisposals.Requests;

namespace UAMS.Application.Validators.AssetDisposals;

public class RejectAssetDisposalRequestValidator
    : AbstractValidator<RejectAssetDisposalRequestDto>
{
    public RejectAssetDisposalRequestValidator()
    {
        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Rejection reason is required.")
            .MaximumLength(2000)
            .WithMessage("Rejection reason must not exceed 2000 characters.");
    }
}