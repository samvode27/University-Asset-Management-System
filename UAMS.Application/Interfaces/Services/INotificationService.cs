using UAMS.Application.DTOs.Notifications.Requests;
using UAMS.Application.DTOs.Notifications.Responses;

namespace UAMS.Application.Interfaces.Services;

public interface INotificationService
{
    // ============================================================
    // GET BY ID
    // ============================================================

    Task<NotificationResponseDto> GetByIdAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken = default);


    // ============================================================
    // GET DETAILS
    // ============================================================

    Task<NotificationDetailResponseDto> GetDetailsAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken = default);


    // ============================================================
    // GET ALL / FILTER / PAGINATION
    // ============================================================

    Task<NotificationListResponseDto> GetAllAsync(
        NotificationFilterRequestDto request,
        Guid currentUserId,
        CancellationToken cancellationToken = default);


    // ============================================================
    // GET USER NOTIFICATIONS
    // ============================================================

    Task<IReadOnlyList<NotificationResponseDto>>
        GetByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default);


    // ============================================================
    // GET UNREAD
    // ============================================================

    Task<IReadOnlyList<NotificationResponseDto>>
        GetUnreadAsync(
            Guid userId,
            CancellationToken cancellationToken = default);


    // ============================================================
    // GET READ
    // ============================================================

    Task<IReadOnlyList<NotificationResponseDto>>
        GetReadAsync(
            Guid userId,
            CancellationToken cancellationToken = default);


    // ============================================================
    // GET HIGH PRIORITY
    // ============================================================

    Task<IReadOnlyList<NotificationResponseDto>>
        GetHighPriorityAsync(
            Guid userId,
            CancellationToken cancellationToken = default);


    // ============================================================
    // GET ACTIVE
    // ============================================================

    Task<IReadOnlyList<NotificationResponseDto>>
        GetActiveAsync(
            Guid userId,
            CancellationToken cancellationToken = default);


    // ============================================================
    // GET BY REFERENCE
    // ============================================================

    Task<IReadOnlyList<NotificationResponseDto>>
        GetByReferenceAsync(
            Guid referenceId,
            string referenceType,
            CancellationToken cancellationToken = default);


    // ============================================================
    // GET UNREAD COUNT
    // ============================================================

    Task<int> GetUnreadCountAsync(
        Guid userId,
        CancellationToken cancellationToken = default);


    // ============================================================
    // CREATE
    // ============================================================

    Task<NotificationResponseDto> CreateAsync(
        CreateNotificationRequestDto request,
        CancellationToken cancellationToken = default);


    // ============================================================
    // MARK AS READ
    // ============================================================

    Task<NotificationResponseDto> MarkAsReadAsync(
        MarkNotificationAsReadRequestDto request,
        Guid userId,
        CancellationToken cancellationToken = default);
}