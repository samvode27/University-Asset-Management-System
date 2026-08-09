using UAMS.Domain.Common;
using UAMS.Domain.Entities.Users;
using UAMS.Domain.Enums;

namespace UAMS.Domain.Entities.Notifications;

public class Notification : AuditableEntity
{
    private Notification()
    {
    }

    public Notification(
        Guid userId,
        string title,
        string message,
        NotificationType type,
        NotificationPriority priority,
        Guid? referenceId,
        string? referenceType,
        string? actionUrl,
        DateTime? expiresAt)
    {
        UserId = userId;
        Title = title;
        Message = message;
        Type = type;
        Priority = priority;
        ReferenceId = referenceId;
        ReferenceType = referenceType;
        ActionUrl = actionUrl;
        ExpiresAt = expiresAt;

        Status = NotificationStatus.Unread;
        IsActive = true;
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

    public bool IsActive { get; private set; }


    public User User { get; private set; } = null!;


    public void Update(
        string title,
        string message,
        NotificationPriority priority,
        string? actionUrl)
    {
        Title = title;
        Message = message;
        Priority = priority;
        ActionUrl = actionUrl;
    }


    public void MarkAsRead()
    {
        Status = NotificationStatus.Read;
        ReadAt = DateTime.UtcNow;
    }


    public void MarkAsUnread()
    {
        Status = NotificationStatus.Unread;
        ReadAt = null;
    }


    public void Archive()
    {
        Status = NotificationStatus.Archived;
    }


    public void Expire()
    {
        if (ExpiresAt.HasValue &&
            ExpiresAt.Value <= DateTime.UtcNow)
        {
            IsActive = false;
        }
    }


    public void Activate()
    {
        IsActive = true;
    }


    public void Deactivate()
    {
        IsActive = false;
    }


    public void MarkDeleted(Guid deletedBy)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
    }
}