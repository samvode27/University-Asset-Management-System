using FluentValidation;
using UAMS.Application.DTOs.Users.Requests;

namespace UAMS.Application.Validators.Users;

public class UserFilterRequestValidator
    : AbstractValidator<UserFilterRequestDto>
{
    public UserFilterRequestValidator()
    {
        RuleFor(x => x.Search)
            .MaximumLength(255)
            .WithMessage("Search text cannot exceed 255 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Search));

        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than zero.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than zero.")
            .LessThanOrEqualTo(100)
            .WithMessage("Page size cannot exceed 100.");
    }
}