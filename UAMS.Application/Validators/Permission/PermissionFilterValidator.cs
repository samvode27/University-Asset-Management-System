using FluentValidation;
using UAMS.Application.DTOs.Permission.Requests;

namespace UAMS.Application.Validators.Permissions;

public class PermissionFilterValidator
    : AbstractValidator<PermissionFilterRequestDto>
{
    public PermissionFilterValidator()
    {
        // ============================================================
        // Name
        // ============================================================

        RuleFor(x => x.Name)
            .MaximumLength(100)
            .WithMessage("Permission name filter cannot exceed 100 characters.");


        // ============================================================
        // Code
        // ============================================================

        RuleFor(x => x.Code)
            .MaximumLength(150)
            .WithMessage("Permission code filter cannot exceed 150 characters.");


        // ============================================================
        // Module
        // ============================================================

        RuleFor(x => x.Module)
            .MaximumLength(100)
            .WithMessage("Permission module filter cannot exceed 100 characters.");


        // ============================================================
        // Search Term
        // ============================================================

        RuleFor(x => x.SearchTerm)
            .MaximumLength(200)
            .WithMessage("Search term cannot exceed 200 characters.");


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