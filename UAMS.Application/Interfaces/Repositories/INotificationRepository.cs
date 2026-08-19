using UAMS.Domain.Entities.Notifications;
using UAMS.Domain.Enums;

namespace UAMS.Application.Interfaces.Repositories;

public interface INotificationRepository
    : IRepository<Notification>
{
    // ================================================================
    // Get Notifications By User
    // ================================================================

    Task<IReadOnlyList<Notification>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Notification By ID For User
    // ================================================================

    Task<Notification?> GetByIdForUserAsync(
        Guid notificationId,
        Guid userId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Notifications By Status
    // ================================================================

    Task<IReadOnlyList<Notification>> GetByStatusAsync(
        NotificationStatus status,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Notifications By Type
    // ================================================================

    Task<IReadOnlyList<Notification>> GetByTypeAsync(
        NotificationType type,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Notifications By Priority
    // ================================================================

    Task<IReadOnlyList<Notification>> GetByPriorityAsync(
        NotificationPriority priority,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Unread Notifications By User
    // ================================================================

    Task<IReadOnlyList<Notification>> GetUnreadByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Read Notifications By User
    // ================================================================

    Task<IReadOnlyList<Notification>> GetReadByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get High Priority Notifications By User
    // ================================================================

    Task<IReadOnlyList<Notification>> GetHighPriorityByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Notifications By Reference
    // ================================================================

    Task<IReadOnlyList<Notification>> GetByReferenceAsync(
        Guid referenceId,
        string referenceType,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Active Notifications By User
    // ================================================================

    Task<IReadOnlyList<Notification>> GetActiveByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Unread Notification Count By User
    // ================================================================

    Task<int> GetUnreadCountByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}