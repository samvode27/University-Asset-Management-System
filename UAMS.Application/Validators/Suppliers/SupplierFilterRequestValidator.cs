using FluentValidation;
using UAMS.Application.DTOs.Suppliers.Requests;

namespace UAMS.Application.Validators.Suppliers;

public class SupplierFilterRequestValidator
    : AbstractValidator<SupplierFilterRequestDto>
{
    public SupplierFilterRequestValidator()
    {
        // ============================================================
        // Search
        // ============================================================

        RuleFor(x => x.Search)
            .MaximumLength(200)
            .WithMessage("Search text must not exceed 200 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Search));


        // ============================================================
        // Page Number
        // ============================================================

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be greater than or equal to 1.");


        // ============================================================
        // Page Size
        // ============================================================

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100.");
    }
}