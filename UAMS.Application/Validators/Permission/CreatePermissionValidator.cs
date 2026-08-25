using FluentValidation;
using UAMS.Application.DTOs.Permission.Requests;

namespace UAMS.Application.Validators.Permissions;

public class CreatePermissionValidator
    : AbstractValidator<CreatePermissionRequestDto>
{
    public CreatePermissionValidator()
    {
        // ============================================================
        // Name
        // ============================================================

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Permission name is required.")
            .MaximumLength(100)
            .WithMessage("Permission name cannot exceed 100 characters.");


        // ============================================================
        // Code
        // ============================================================

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Permission code is required.")
            .MaximumLength(150)
            .WithMessage("Permission code cannot exceed 150 characters.");


        // ============================================================
        // Description
        // ============================================================

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Permission description cannot exceed 500 characters.");


        // ============================================================
        // Module
        // ============================================================

        RuleFor(x => x.Module)
            .NotEmpty()
            .WithMessage("Permission module is required.")
            .MaximumLength(100)
            .WithMessage("Permission module cannot exceed 100 characters.");


        // ============================================================
        // Created By
        // ============================================================

        RuleFor(x => x.CreatedBy)
            .Must(id => !id.HasValue || id.Value != Guid.Empty)
            .WithMessage("CreatedBy must be a valid user ID.");
    }
}