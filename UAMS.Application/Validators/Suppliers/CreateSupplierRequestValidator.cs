using FluentValidation;
using UAMS.Application.DTOs.Suppliers.Requests;

namespace UAMS.Application.Validators.Suppliers;

public class CreateSupplierRequestValidator
    : AbstractValidator<CreateSupplierRequestDto>
{
    public CreateSupplierRequestValidator()
    {
        // ============================================================
        // Name
        // ============================================================

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Supplier name is required.")
            .MaximumLength(200)
            .WithMessage("Supplier name must not exceed 200 characters.");


        // ============================================================
        // Code
        // ============================================================

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Supplier code is required.")
            .MaximumLength(50)
            .WithMessage("Supplier code must not exceed 50 characters.");


        // ============================================================
        // Contact Person
        // ============================================================

        RuleFor(x => x.ContactPerson)
            .MaximumLength(200)
            .WithMessage("Contact person must not exceed 200 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ContactPerson));


        // ============================================================
        // Phone Number
        // ============================================================

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(30)
            .WithMessage("Phone number must not exceed 30 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));


        // ============================================================
        // Email
        // ============================================================

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Please provide a valid email address.")
            .MaximumLength(255)
            .WithMessage("Email must not exceed 255 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));


        // ============================================================
        // Address
        // ============================================================

        RuleFor(x => x.Address)
            .MaximumLength(500)
            .WithMessage("Address must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Address));


        // ============================================================
        // Tax Identification Number
        // ============================================================

        RuleFor(x => x.TaxIdentificationNumber)
            .MaximumLength(100)
            .WithMessage("Tax identification number must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.TaxIdentificationNumber));
    }
}