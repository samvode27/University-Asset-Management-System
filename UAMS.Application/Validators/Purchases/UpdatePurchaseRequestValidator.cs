using FluentValidation;
using UAMS.Application.DTOs.Purchases.Requests;

namespace UAMS.Application.Validators.Purchases;

public class UpdatePurchaseRequestValidator
    : AbstractValidator<UpdatePurchaseRequestDto>
{
    public UpdatePurchaseRequestValidator()
    {
        RuleFor(x => x.SupplierId)
            .NotEmpty()
            .WithMessage("Supplier is required.");

        RuleFor(x => x.PurchaseDate)
            .NotEmpty()
            .WithMessage("Purchase date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Purchase date cannot be in the future.");

        RuleFor(x => x.InvoiceNumber)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.InvoiceNumber))
            .WithMessage("Invoice number cannot exceed 100 characters.");

        RuleFor(x => x.PurchaseOrderNumber)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.PurchaseOrderNumber))
            .WithMessage("Purchase order number cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(2000)
            .When(x => !string.IsNullOrWhiteSpace(x.Description))
            .WithMessage("Description cannot exceed 2000 characters.");

        RuleFor(x => x.TotalAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Total amount cannot be negative.");

        RuleFor(x => x.Currency)
            .NotEmpty()
            .WithMessage("Currency is required.")
            .MaximumLength(10)
            .WithMessage("Currency cannot exceed 10 characters.");
    }
}