using FluentValidation;
using UAMS.Application.DTOs.AssetReturns.Requests;

namespace UAMS.Application.Validators.AssetReturns;

public class UpdateAssetReturnRequestValidator
    : AbstractValidator<UpdateAssetReturnRequestDto>
{
    public UpdateAssetReturnRequestValidator()
    {
        RuleFor(x => x.ReturnDate)
            .NotEmpty()
            .WithMessage("Return date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Return date cannot be in the future.");

        RuleFor(x => x.ReturnLocation)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.ReturnLocation))
            .WithMessage("Return location cannot exceed 500 characters.");

        RuleFor(x => x.Condition)
            .IsInEnum()
            .WithMessage("Invalid asset return condition.");

        RuleFor(x => x.InspectionNotes)
            .MaximumLength(2000)
            .When(x => !string.IsNullOrWhiteSpace(x.InspectionNotes))
            .WithMessage("Inspection notes cannot exceed 2000 characters.");

        RuleFor(x => x.Remarks)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.Remarks))
            .WithMessage("Remarks cannot exceed 1000 characters.");
    }
}