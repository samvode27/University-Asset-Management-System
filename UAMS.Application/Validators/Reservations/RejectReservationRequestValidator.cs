using FluentValidation;
using UAMS.Application.DTOs.Reservations.Requests;

namespace UAMS.Application.Validators.Reservations;

public class RejectReservationRequestValidator
    : AbstractValidator<RejectReservationRequestDto>
{
    public RejectReservationRequestValidator()
    {
        RuleFor(x => x.RejectionReason)
            .NotEmpty()
            .WithMessage("Rejection reason is required.")
            .MaximumLength(1000)
            .WithMessage("Rejection reason cannot exceed 1000 characters.");
    }
}