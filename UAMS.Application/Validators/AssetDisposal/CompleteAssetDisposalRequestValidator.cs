using FluentValidation;
using UAMS.Application.DTOs.AssetDisposals.Requests;

namespace UAMS.Application.Validators.AssetDisposals;

public class CompleteAssetDisposalRequestValidator
    : AbstractValidator<CompleteAssetDisposalRequestDto>
{
    public CompleteAssetDisposalRequestValidator()
    {
        RuleFor(x => x.DisposalMethod)
            .IsInEnum()
            .WithMessage("A valid disposal method is required.");

        RuleFor(x => x.DisposalValue)
            .GreaterThanOrEqualTo(0)
            .When(x => x.DisposalValue.HasValue)
            .WithMessage("Disposal value cannot be negative.");

        RuleFor(x => x.Remarks)
            .MaximumLength(2000)
            .When(x => !string.IsNullOrWhiteSpace(x.Remarks))
            .WithMessage("Remarks must not exceed 2000 characters.");
    }
}