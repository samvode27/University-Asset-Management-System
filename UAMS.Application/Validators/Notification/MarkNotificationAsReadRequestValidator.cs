using FluentValidation;
using UAMS.Application.DTOs.Notifications.Requests;

namespace UAMS.Application.Validators.Notifications;

public class MarkNotificationAsReadRequestValidator
    : AbstractValidator<MarkNotificationAsReadRequestDto>
{
    public MarkNotificationAsReadRequestValidator()
    {
        // ============================================================
        // Notification
        // ============================================================

        RuleFor(x => x.NotificationId)
            .NotEmpty()
            .WithMessage("Notification ID is required.");
    }
}