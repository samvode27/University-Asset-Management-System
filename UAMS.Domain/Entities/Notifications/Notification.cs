using UAMS.Domain.Common;
using UAMS.Domain.Entities.Users;
using UAMS.Domain.Enums;

namespace UAMS.Domain.Entities.Notifications;

public class Notification : AuditableEntity
{
    private Notification()
    {
    }

    public Guid UserId { get; private set; }

    public string Title { get; private set; } = null!;

    public string Message { get; private set; } = null!;

    public NotificationType Type { get; private set; }

    public NotificationPriority Priority { get; private set; }

    public NotificationStatus Status { get; private set; }

    public Guid? ReferenceId { get; private set; }

    public string? ReferenceType { get; private set; }

    public string? ActionUrl { get; private set; }

    public DateTime? ReadAt { get; private set; }

    public DateTime? ExpiresAt { get; private set; }

    public User User { get; private set; } = null!;


    // ============================================================
    // CREATE
    // ============================================================

    public static Notification Create(
        Guid userId,
        string title,
        string message,
        NotificationType type,
        NotificationPriority priority,
        Guid? referenceId = null,
        string? referenceType = null,
        string? actionUrl = null,
        DateTime? expiresAt = null)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException(
                "User ID is required.",
                nameof(userId));

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException(
                "Notification title is required.",
                nameof(title));

        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException(
                "Notification message is required.",
                nameof(message));

        if (referenceId == Guid.Empty)
            referenceId = null;

        if (expiresAt.HasValue &&
            expiresAt.Value <= DateTime.UtcNow)
        {
            throw new ArgumentException(
                "Expiration date must be in the future.",
                nameof(expiresAt));
        }

        return new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Title = title.Trim(),
            Message = message.Trim(),
            Type = type,
            Priority = priority,
            Status = NotificationStatus.Unread,
            ReferenceId = referenceId,
            ReferenceType = string.IsNullOrWhiteSpace(referenceType)
                ? null
                : referenceType.Trim(),
            ActionUrl = string.IsNullOrWhiteSpace(actionUrl)
                ? null
                : actionUrl.Trim(),
            ExpiresAt = expiresAt
        };
    }


    // ============================================================
    // MARK AS READ
    // ============================================================

    public void MarkAsRead()
    {
        if (Status == NotificationStatus.Archived)
            throw new InvalidOperationException(
                "Archived notifications cannot be marked as read.");

        if (Status == NotificationStatus.Read)
            return;

        Status = NotificationStatus.Read;
        ReadAt = DateTime.UtcNow;
    }
}