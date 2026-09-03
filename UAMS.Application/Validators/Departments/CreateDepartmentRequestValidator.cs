using FluentValidation;
using UAMS.Application.DTOs.Departments.Requests;

namespace UAMS.Application.Validators.Departments;

public class CreateDepartmentRequestValidator
    : AbstractValidator<CreateDepartmentRequestDto>
{
    public CreateDepartmentRequestValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Department code is required.")
            .MaximumLength(20)
            .WithMessage("Department code cannot exceed 20 characters.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Department name is required.")
            .MaximumLength(150)
            .WithMessage("Department name cannot exceed 150 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description cannot exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.OfficeLocation)
            .MaximumLength(250)
            .WithMessage("Office location cannot exceed 250 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.OfficeLocation));

        RuleFor(x => x.EstablishedDate)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Established date cannot be in the future.")
            .When(x => x.EstablishedDate.HasValue);
    }
}