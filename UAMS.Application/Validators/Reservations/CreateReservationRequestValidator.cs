using FluentValidation;
using UAMS.Application.DTOs.Reservations.Requests;

namespace UAMS.Application.Validators.Reservations;

public class CreateReservationRequestValidator
    : AbstractValidator<CreateReservationRequestDto>
{
    public CreateReservationRequestValidator()
    {
        // ============================================================
        // Asset
        // ============================================================

        RuleFor(x => x.AssetId)
            .NotEmpty()
            .WithMessage("AssetId is required.");


        // ============================================================
        // Reservation Start
        // ============================================================

        RuleFor(x => x.ReservationStartDate)
            .NotEmpty()
            .WithMessage("Reservation start date is required.");


        // ============================================================
        // Reservation End
        // ============================================================

        RuleFor(x => x.ReservationEndDate)
            .NotEmpty()
            .WithMessage("Reservation end date is required.");

        RuleFor(x => x.ReservationEndDate)
            .GreaterThan(x => x.ReservationStartDate)
            .WithMessage(
                "Reservation end date must be later than the reservation start date.");


        // ============================================================
        // Purpose
        // ============================================================

        RuleFor(x => x.Purpose)
            .MaximumLength(1000)
            .WithMessage("Purpose cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Purpose));


        // ============================================================
        // Remarks
        // ============================================================

        RuleFor(x => x.Remarks)
            .MaximumLength(1000)
            .WithMessage("Remarks cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Remarks));
    }
}