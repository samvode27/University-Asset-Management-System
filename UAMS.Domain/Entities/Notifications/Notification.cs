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

}