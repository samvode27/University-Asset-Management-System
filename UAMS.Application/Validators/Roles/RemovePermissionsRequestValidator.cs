using FluentValidation;
using UAMS.Application.DTOs.Roles.Requests;

namespace UAMS.Application.Validators.Roles;

public class RemovePermissionsRequestValidator
    : AbstractValidator<RemovePermissionsRequestDto>
{
    public RemovePermissionsRequestValidator()
    {
        RuleFor(x => x.PermissionIds)
            .NotEmpty()
            .WithMessage("At least one permission must be provided.");

        RuleForEach(x => x.PermissionIds)
            .NotEmpty()
            .WithMessage("Permission ID cannot be empty.");

        RuleFor(x => x.PermissionIds)
            .Must(ids => ids.Distinct().Count() == ids.Count())
            .WithMessage("Duplicate permission IDs are not allowed.");
    }
}