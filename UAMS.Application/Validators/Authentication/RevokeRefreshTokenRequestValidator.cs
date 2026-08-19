using FluentValidation;
using UAMS.Application.DTOs.Authentication.Requests;

namespace UAMS.Application.Validators.Authentication;

public class RevokeRefreshTokenRequestValidator
    : AbstractValidator<RevokeRefreshTokenRequestDto>
{
    public RevokeRefreshTokenRequestValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token is required.");
    }
}