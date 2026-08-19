using Microsoft.EntityFrameworkCore;
using UAMS.Application.Interfaces.Repositories;
using UAMS.Domain.Entities.Notifications;
using UAMS.Domain.Enums;
using UAMS.Infrastructure.Persistence;

namespace UAMS.Infrastructure.Repositories;

public class NotificationRepository
    : GenericRepository<Notification>, INotificationRepository
{
    public NotificationRepository(UAMSDbContext context)
        : base(context)
    {
    }


    // ================================================================
    // Get Notifications By User
    // ================================================================

    public virtual async Task<IReadOnlyList<Notification>>
        GetByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(notification =>
                notification.UserId == userId)
            .OrderByDescending(notification =>
                notification.CreatedAt)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Notification By ID For User
    // ================================================================

    public virtual async Task<Notification?>
        GetByIdForUserAsync(
            Guid notificationId,
            Guid userId,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                notification =>
                    notification.Id == notificationId &&
                    notification.UserId == userId,
                cancellationToken);
    }


    // ================================================================
    // Get Notifications By Status
    // ================================================================

    public virtual async Task<IReadOnlyList<Notification>>
        GetByStatusAsync(
            NotificationStatus status,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(notification =>
                notification.Status == status)
            .OrderByDescending(notification =>
                notification.CreatedAt)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Notifications By Type
    // ================================================================

    public virtual async Task<IReadOnlyList<Notification>>
        GetByTypeAsync(
            NotificationType type,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(notification =>
                notification.Type == type)
            .OrderByDescending(notification =>
                notification.CreatedAt)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Notifications By Priority
    // ================================================================

    public virtual async Task<IReadOnlyList<Notification>>
        GetByPriorityAsync(
            NotificationPriority priority,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(notification =>
                notification.Priority == priority)
            .OrderByDescending(notification =>
                notification.CreatedAt)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Unread Notifications By User
    // ================================================================

    public virtual async Task<IReadOnlyList<Notification>>
        GetUnreadByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(notification =>
                notification.UserId == userId &&
                notification.Status == NotificationStatus.Unread)
            .OrderByDescending(notification =>
                notification.CreatedAt)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Read Notifications By User
    // ================================================================

    public virtual async Task<IReadOnlyList<Notification>>
        GetReadByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(notification =>
                notification.UserId == userId &&
                notification.Status == NotificationStatus.Read)
            .OrderByDescending(notification =>
                notification.CreatedAt)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get High Priority Notifications By User
    // ================================================================

    public virtual async Task<IReadOnlyList<Notification>>
        GetHighPriorityByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(notification =>
                notification.UserId == userId &&
                notification.Priority == NotificationPriority.High)
            .OrderByDescending(notification =>
                notification.CreatedAt)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Notifications By Reference
    // ================================================================

    public virtual async Task<IReadOnlyList<Notification>>
        GetByReferenceAsync(
            Guid referenceId,
            string referenceType,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(notification =>
                notification.ReferenceId == referenceId &&
                notification.ReferenceType == referenceType)
            .OrderByDescending(notification =>
                notification.CreatedAt)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Active Notifications By User
    // ================================================================

    public virtual async Task<IReadOnlyList<Notification>>
        GetActiveByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        return await DbSet
            .AsNoTracking()
            .Where(notification =>
                notification.UserId == userId &&
                notification.Status != NotificationStatus.Archived &&
                (
                    notification.ExpiresAt == null ||
                    notification.ExpiresAt > now
                ))
            .OrderByDescending(notification =>
                notification.CreatedAt)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Unread Notification Count By User
    // ================================================================

    public virtual async Task<int>
        GetUnreadCountByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        return await DbSet
            .CountAsync(
                notification =>
                    notification.UserId == userId &&
                    notification.Status == NotificationStatus.Unread &&
                    (
                        notification.ExpiresAt == null ||
                        notification.ExpiresAt > now
                    ),
                cancellationToken);
    }
}