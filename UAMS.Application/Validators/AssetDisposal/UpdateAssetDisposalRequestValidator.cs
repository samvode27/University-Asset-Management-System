using FluentValidation;
using UAMS.Application.DTOs.AssetDisposals.Requests;

namespace UAMS.Application.Validators.AssetDisposals;

public class UpdateAssetDisposalRequestValidator
    : AbstractValidator<UpdateAssetDisposalRequestDto>
{
    public UpdateAssetDisposalRequestValidator()
    {
        RuleFor(x => x.MaintenanceId)
            .Must(id => id == null || id != Guid.Empty)
            .WithMessage("Maintenance ID must be a valid identifier.");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Disposal reason is required.")
            .MaximumLength(1000)
            .WithMessage("Disposal reason must not exceed 1000 characters.");

        RuleFor(x => x.BookValue)
            .GreaterThanOrEqualTo(0)
            .When(x => x.BookValue.HasValue)
            .WithMessage("Book value cannot be negative.");

        RuleFor(x => x.EstimatedValue)
            .GreaterThanOrEqualTo(0)
            .When(x => x.EstimatedValue.HasValue)
            .WithMessage("Estimated value cannot be negative.");

        RuleFor(x => x.Remarks)
            .MaximumLength(2000)
            .When(x => !string.IsNullOrWhiteSpace(x.Remarks))
            .WithMessage("Remarks must not exceed 2000 characters.");
    }
}