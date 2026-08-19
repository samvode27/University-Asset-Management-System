using FluentValidation;
using UAMS.Application.DTOs.Users.Requests;

namespace UAMS.Application.Validators.Users;

public class AssignRoleRequestValidator
    : AbstractValidator<AssignRoleRequestDto>
{
    public AssignRoleRequestValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty()
            .WithMessage("Role ID is required.");
    }
}