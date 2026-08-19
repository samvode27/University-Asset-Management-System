using FluentValidation;
using UAMS.Application.DTOs.AssetDisposals.Requests;

namespace UAMS.Application.Validators.AssetDisposals;

public class AssetDisposalFilterRequestValidator
    : AbstractValidator<AssetDisposalFilterRequestDto>
{
    public AssetDisposalFilterRequestValidator()
    {
        RuleFor(x => x.SearchTerm)
            .MaximumLength(200)
            .When(x => !string.IsNullOrWhiteSpace(x.SearchTerm))
            .WithMessage("Search term must not exceed 200 characters.");

        // ============================================================
        // Requested Date Range
        // ============================================================

        RuleFor(x => x)
            .Must(x =>
                !x.RequestedFromDate.HasValue ||
                !x.RequestedToDate.HasValue ||
                x.RequestedFromDate.Value <= x.RequestedToDate.Value)
            .WithMessage(
                "Requested from date must be earlier than or equal to requested to date.");

        // ============================================================
        // Approved Date Range
        // ============================================================

        RuleFor(x => x)
            .Must(x =>
                !x.ApprovedFromDate.HasValue ||
                !x.ApprovedToDate.HasValue ||
                x.ApprovedFromDate.Value <= x.ApprovedToDate.Value)
            .WithMessage(
                "Approved from date must be earlier than or equal to approved to date.");

        // ============================================================
        // Disposal Date Range
        // ============================================================

        RuleFor(x => x)
            .Must(x =>
                !x.DisposalFromDate.HasValue ||
                !x.DisposalToDate.HasValue ||
                x.DisposalFromDate.Value <= x.DisposalToDate.Value)
            .WithMessage(
                "Disposal from date must be earlier than or equal to disposal to date.");

        // ============================================================
        // Pagination
        // ============================================================

        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100.");
    }
}