using FluentValidation;
using UAMS.Application.DTOs.Reservations.Requests;

namespace UAMS.Application.Validators.Reservations;

public class ApproveReservationRequestValidator
    : AbstractValidator<ApproveReservationRequestDto>
{
    public ApproveReservationRequestValidator()
    {
        RuleFor(x => x.ApprovalRemarks)
            .MaximumLength(1000)
            .WithMessage("Approval remarks cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ApprovalRemarks));
    }
}