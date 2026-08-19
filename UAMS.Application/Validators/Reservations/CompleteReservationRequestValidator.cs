using FluentValidation;
using UAMS.Application.DTOs.Reservations.Requests;

namespace UAMS.Application.Validators.Reservations;

public class CompleteReservationRequestValidator
    : AbstractValidator<CompleteReservationRequestDto>
{
    public CompleteReservationRequestValidator()
    {
        RuleFor(x => x.Remarks)
            .MaximumLength(1000)
            .WithMessage("Remarks cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Remarks));
    }
}