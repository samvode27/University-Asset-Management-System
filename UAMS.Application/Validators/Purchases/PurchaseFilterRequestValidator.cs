using FluentValidation;
using UAMS.Application.DTOs.Purchases.Requests;

namespace UAMS.Application.Validators.Purchases;

public class PurchaseFilterRequestValidator
    : AbstractValidator<PurchaseFilterRequestDto>
{
    public PurchaseFilterRequestValidator()
    {
        RuleFor(x => x.Search)
            .MaximumLength(200)
            .When(x => !string.IsNullOrWhiteSpace(x.Search))
            .WithMessage("Search cannot exceed 200 characters.");

        RuleFor(x => x.InvoiceNumber)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.InvoiceNumber))
            .WithMessage("Invoice number cannot exceed 100 characters.");

        RuleFor(x => x.PurchaseOrderNumber)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.PurchaseOrderNumber))
            .WithMessage("Purchase order number cannot exceed 100 characters.");

        RuleFor(x => x.Currency)
            .MaximumLength(10)
            .When(x => !string.IsNullOrWhiteSpace(x.Currency))
            .WithMessage("Currency cannot exceed 10 characters.");

        RuleFor(x => x.PurchaseDateTo)
            .GreaterThanOrEqualTo(x => x.PurchaseDateFrom)
            .When(x =>
                x.PurchaseDateFrom.HasValue &&
                x.PurchaseDateTo.HasValue)
            .WithMessage("Purchase date to cannot be earlier than purchase date from.");

        RuleFor(x => x.MaximumAmount)
            .GreaterThanOrEqualTo(x => x.MinimumAmount)
            .When(x =>
                x.MinimumAmount.HasValue &&
                x.MaximumAmount.HasValue)
            .WithMessage("Maximum amount cannot be less than minimum amount.");

        RuleFor(x => x.MinimumAmount)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MinimumAmount.HasValue)
            .WithMessage("Minimum amount cannot be negative.");

        RuleFor(x => x.MaximumAmount)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MaximumAmount.HasValue)
            .WithMessage("Maximum amount cannot be negative.");

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100.");
    }
}