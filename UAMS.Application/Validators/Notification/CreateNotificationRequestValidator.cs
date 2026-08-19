using FluentValidation;
using UAMS.Application.DTOs.Notifications.Requests;

namespace UAMS.Application.Validators.Notifications;

public class CreateNotificationRequestValidator
    : AbstractValidator<CreateNotificationRequestDto>
{
    public CreateNotificationRequestValidator()
    {
        // ============================================================
        // Recipient
        // ============================================================

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.");


        // ============================================================
        // Notification Content
        // ============================================================

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Notification title is required.")
            .MaximumLength(250)
            .WithMessage("Notification title cannot exceed 250 characters.");

        RuleFor(x => x.Message)
            .NotEmpty()
            .WithMessage("Notification message is required.")
            .MaximumLength(2000)
            .WithMessage("Notification message cannot exceed 2000 characters.");


        // ============================================================
        // Classification
        // ============================================================

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Invalid notification type.");

        RuleFor(x => x.Priority)
            .IsInEnum()
            .WithMessage("Invalid notification priority.");


        // ============================================================
        // Reference
        // ============================================================

        RuleFor(x => x.ReferenceType)
            .MaximumLength(100)
            .WithMessage("Reference type cannot exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ReferenceType));

        RuleFor(x => x.ReferenceId)
            .NotEqual(Guid.Empty)
            .WithMessage("Reference ID must be a valid identifier.")
            .When(x => x.ReferenceId.HasValue);


        // ============================================================
        // Action
        // ============================================================

        RuleFor(x => x.ActionUrl)
            .MaximumLength(500)
            .WithMessage("Action URL cannot exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ActionUrl));


        // ============================================================
        // Expiration
        // ============================================================

        RuleFor(x => x.ExpiresAt)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Expiration date must be in the future.")
            .When(x => x.ExpiresAt.HasValue);
    }
}