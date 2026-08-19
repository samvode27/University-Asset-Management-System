using FluentValidation;
using UAMS.Application.DTOs.Notifications.Requests;

namespace UAMS.Application.Validators.Notifications;

public class NotificationFilterRequestValidator
    : AbstractValidator<NotificationFilterRequestDto>
{
    public NotificationFilterRequestValidator()
    {
        // ============================================================
        // Search
        // ============================================================

        RuleFor(x => x.SearchTerm)
            .MaximumLength(500)
            .WithMessage("Search term cannot exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.SearchTerm));


        // ============================================================
        // User
        // ============================================================

        RuleFor(x => x.UserId)
            .NotEqual(Guid.Empty)
            .WithMessage("User ID must be a valid identifier.")
            .When(x => x.UserId.HasValue);


        // ============================================================
        // Notification Type
        // ============================================================

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Invalid notification type.")
            .When(x => x.Type.HasValue);


        // ============================================================
        // Priority
        // ============================================================

        RuleFor(x => x.Priority)
            .IsInEnum()
            .WithMessage("Invalid notification priority.")
            .When(x => x.Priority.HasValue);


        // ============================================================
        // Status
        // ============================================================

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Invalid notification status.")
            .When(x => x.Status.HasValue);


        // ============================================================
        // Reference
        // ============================================================

        RuleFor(x => x.ReferenceId)
            .NotEqual(Guid.Empty)
            .WithMessage("Reference ID must be a valid identifier.")
            .When(x => x.ReferenceId.HasValue);

        RuleFor(x => x.ReferenceType)
            .MaximumLength(100)
            .WithMessage("Reference type cannot exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ReferenceType));


        // ============================================================
        // Date Range
        // ============================================================

        RuleFor(x => x.FromDate)
            .LessThanOrEqualTo(x => x.ToDate)
            .WithMessage("From date must be earlier than or equal to To date.")
            .When(x => x.FromDate.HasValue && x.ToDate.HasValue);


        // ============================================================
        // Pagination
        // ============================================================

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100.");
    }
}