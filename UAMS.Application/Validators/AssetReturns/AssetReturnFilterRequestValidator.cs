using FluentValidation;
using UAMS.Application.DTOs.AssetReturns.Requests;

namespace UAMS.Application.Validators.AssetReturns;

public class AssetReturnFilterRequestValidator
    : AbstractValidator<AssetReturnFilterRequestDto>
{
    public AssetReturnFilterRequestValidator()
    {
        RuleFor(x => x.ReturnNumber)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.ReturnNumber))
            .WithMessage("Return number cannot exceed 100 characters.");

        RuleFor(x => x.Condition)
            .IsInEnum()
            .When(x => x.Condition.HasValue)
            .WithMessage("Invalid asset return condition.");

        RuleFor(x => x.Status)
            .IsInEnum()
            .When(x => x.Status.HasValue)
            .WithMessage("Invalid asset return status.");

        RuleFor(x => x.ReturnDateFrom)
            .LessThanOrEqualTo(x => x.ReturnDateTo)
            .When(x => x.ReturnDateFrom.HasValue &&
                       x.ReturnDateTo.HasValue)
            .WithMessage(
                "Return date from must be before or equal to return date to.");

        RuleFor(x => x.ReturnDateTo)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .When(x => x.ReturnDateTo.HasValue)
            .WithMessage("Return date to cannot be in the future.");

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100.");

        RuleFor(x => x.SearchTerm)
            .MaximumLength(200)
            .When(x => !string.IsNullOrWhiteSpace(x.SearchTerm))
            .WithMessage("Search term cannot exceed 200 characters.");
    }
}