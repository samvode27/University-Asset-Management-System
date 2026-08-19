using FluentValidation;
using UAMS.Application.DTOs.Roles.Requests;

namespace UAMS.Application.Validators.Roles;

public class CreateRoleRequestValidator
    : AbstractValidator<CreateRoleRequestDto>
{
    public CreateRoleRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Role name is required.")
            .MaximumLength(100)
            .WithMessage("Role name cannot exceed 100 characters.");

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Role code is required.")
            .MaximumLength(100)
            .WithMessage("Role code cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Role description cannot exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}