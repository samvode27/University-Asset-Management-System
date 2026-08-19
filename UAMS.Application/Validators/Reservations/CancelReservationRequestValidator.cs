using FluentValidation;
using UAMS.Application.DTOs.Reservations.Requests;

namespace UAMS.Application.Validators.Reservations;

public class CancelReservationRequestValidator
    : AbstractValidator<CancelReservationRequestDto>
{
    public CancelReservationRequestValidator()
    {
        RuleFor(x => x.CancellationReason)
            .MaximumLength(1000)
            .WithMessage("Cancellation reason cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.CancellationReason));
    }
}