using UAMS.Application.DTOs.Notifications.Requests;
using UAMS.Application.DTOs.Notifications.Responses;
using UAMS.Application.Interfaces.Persistence;
using UAMS.Application.Interfaces.Services;
using UAMS.Domain.Entities.Notifications;
using UAMS.Domain.Enums;

namespace UAMS.Application.Services.Notifications;

public class NotificationService : INotificationService
{
    private readonly IUnitOfWork _unitOfWork;

    public NotificationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    // ============================================================
    // GET BY ID
    // ============================================================

    public async Task<NotificationResponseDto> GetByIdAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        ValidateUserId(userId);

        var notification =
            await _unitOfWork.Notifications.GetByIdForUserAsync(
                id,
                userId,
                cancellationToken);

        if (notification is null)
            throw new KeyNotFoundException(
                "Notification record was not found.");

        return MapToResponse(notification);
    }


    // ============================================================
    // GET DETAILS
    // ============================================================

    public async Task<NotificationDetailResponseDto> GetDetailsAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        ValidateUserId(userId);

        var notification =
            await _unitOfWork.Notifications.GetByIdForUserAsync(
                id,
                userId,
                cancellationToken);

        if (notification is null)
            throw new KeyNotFoundException(
                "Notification record was not found.");

        return MapToDetailResponse(notification);
    }


    // ============================================================
    // GET ALL / FILTER / PAGINATION
    // ============================================================

    public async Task<NotificationListResponseDto> GetAllAsync(
        NotificationFilterRequestDto request,
        Guid currentUserId,
        CancellationToken cancellationToken = default)
    {
        ValidateUserId(currentUserId);

        if (request is null)
            throw new ArgumentNullException(nameof(request));

        var notifications =
            await _unitOfWork.Notifications.GetByUserIdAsync(
                currentUserId,
                cancellationToken);

        IEnumerable<Notification> filtered = notifications;


        // ------------------------------------------------------------
        // Search
        // ------------------------------------------------------------

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm =
                request.SearchTerm.Trim();

            filtered = filtered.Where(x =>
                x.Title.Contains(
                    searchTerm,
                    StringComparison.OrdinalIgnoreCase) ||

                x.Message.Contains(
                    searchTerm,
                    StringComparison.OrdinalIgnoreCase) ||

                (x.ReferenceType != null &&
                 x.ReferenceType.Contains(
                     searchTerm,
                     StringComparison.OrdinalIgnoreCase)));
        }


        // ------------------------------------------------------------
        // User
        // ------------------------------------------------------------

        if (request.UserId.HasValue)
        {
            if (request.UserId.Value != currentUserId)
                throw new UnauthorizedAccessException(
                    "You are not authorized to access another user's notifications.");

            filtered = filtered.Where(x =>
                x.UserId == request.UserId.Value);
        }


        // ------------------------------------------------------------
        // Type
        // ------------------------------------------------------------

        if (request.Type.HasValue)
        {
            filtered = filtered.Where(x =>
                x.Type == request.Type.Value);
        }


        // ------------------------------------------------------------
        // Priority
        // ------------------------------------------------------------

        if (request.Priority.HasValue)
        {
            filtered = filtered.Where(x =>
                x.Priority == request.Priority.Value);
        }


        // ------------------------------------------------------------
        // Status
        // ------------------------------------------------------------

        if (request.Status.HasValue)
        {
            filtered = filtered.Where(x =>
                x.Status == request.Status.Value);
        }


        // ------------------------------------------------------------
        // Reference
        // ------------------------------------------------------------

        if (request.ReferenceId.HasValue)
        {
            filtered = filtered.Where(x =>
                x.ReferenceId == request.ReferenceId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.ReferenceType))
        {
            var referenceType =
                request.ReferenceType.Trim();

            filtered = filtered.Where(x =>
                x.ReferenceType != null &&
                x.ReferenceType.Equals(
                    referenceType,
                    StringComparison.OrdinalIgnoreCase));
        }


        // ------------------------------------------------------------
        // Read State
        // ------------------------------------------------------------

        if (request.IsRead.HasValue)
        {
            filtered = request.IsRead.Value
                ? filtered.Where(x =>
                    x.Status ==
                    NotificationStatus.Read)
                : filtered.Where(x =>
                    x.Status ==
                    NotificationStatus.Unread);
        }


        // ------------------------------------------------------------
        // Expiration
        // ------------------------------------------------------------

        if (request.IncludeExpired != true)
        {
            var now = DateTime.UtcNow;

            filtered = filtered.Where(x =>
                !x.ExpiresAt.HasValue ||
                x.ExpiresAt.Value > now);
        }


        // ------------------------------------------------------------
        // Date Range
        // ------------------------------------------------------------

        if (request.FromDate.HasValue)
        {
            filtered = filtered.Where(x =>
                x.CreatedAt >= request.FromDate.Value);
        }

        if (request.ToDate.HasValue)
        {
            filtered = filtered.Where(x =>
                x.CreatedAt <= request.ToDate.Value);
        }


        // ------------------------------------------------------------
        // Ordering
        // ------------------------------------------------------------

        var ordered =
            filtered
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

        var totalCount =
            ordered.Count;

        var totalPages =
            (int)Math.Ceiling(
                totalCount /
                (double)request.PageSize);

        var items =
            ordered
                .Skip(
                    (request.PageNumber - 1) *
                    request.PageSize)
                .Take(request.PageSize)
                .Select(MapToResponse)
                .ToList();

        return new NotificationListResponseDto
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            HasPreviousPage =
                request.PageNumber > 1,
            HasNextPage =
                request.PageNumber < totalPages
        };
    }


    // ============================================================
    // GET BY USER
    // ============================================================

    public async Task<IReadOnlyList<NotificationResponseDto>>
        GetByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
    {
        ValidateUserId(userId);

        var notifications =
            await _unitOfWork.Notifications.GetByUserIdAsync(
                userId,
                cancellationToken);

        return notifications
            .Select(MapToResponse)
            .ToList();
    }


    // ============================================================
    // GET UNREAD
    // ============================================================

    public async Task<IReadOnlyList<NotificationResponseDto>>
        GetUnreadAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
    {
        ValidateUserId(userId);

        var notifications =
            await _unitOfWork.Notifications.GetUnreadByUserIdAsync(
                userId,
                cancellationToken);

        return notifications
            .Select(MapToResponse)
            .ToList();
    }


    // ============================================================
    // GET READ
    // ============================================================

    public async Task<IReadOnlyList<NotificationResponseDto>>
        GetReadAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
    {
        ValidateUserId(userId);

        var notifications =
            await _unitOfWork.Notifications.GetReadByUserIdAsync(
                userId,
                cancellationToken);

        return notifications
            .Select(MapToResponse)
            .ToList();
    }


    // ============================================================
    // GET HIGH PRIORITY
    // ============================================================

    public async Task<IReadOnlyList<NotificationResponseDto>>
        GetHighPriorityAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
    {
        ValidateUserId(userId);

        var notifications =
            await _unitOfWork.Notifications
                .GetHighPriorityByUserIdAsync(
                    userId,
                    cancellationToken);

        return notifications
            .Select(MapToResponse)
            .ToList();
    }


    // ============================================================
    // GET ACTIVE
    // ============================================================

    public async Task<IReadOnlyList<NotificationResponseDto>>
        GetActiveAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
    {
        ValidateUserId(userId);

        var notifications =
            await _unitOfWork.Notifications
                .GetActiveByUserIdAsync(
                    userId,
                    cancellationToken);

        return notifications
            .Select(MapToResponse)
            .ToList();
    }


    // ============================================================
    // GET BY REFERENCE
    // ============================================================

    public async Task<IReadOnlyList<NotificationResponseDto>>
        GetByReferenceAsync(
            Guid referenceId,
            string referenceType,
            CancellationToken cancellationToken = default)
    {
        if (referenceId == Guid.Empty)
            throw new ArgumentException(
                "Reference ID is required.");

        if (string.IsNullOrWhiteSpace(referenceType))
            throw new ArgumentException(
                "Reference type is required.");

        var notifications =
            await _unitOfWork.Notifications.GetByReferenceAsync(
                referenceId,
                referenceType.Trim(),
                cancellationToken);

        return notifications
            .Select(MapToResponse)
            .ToList();
    }


    // ============================================================
    // GET UNREAD COUNT
    // ============================================================

    public async Task<int> GetUnreadCountAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        ValidateUserId(userId);

        return await _unitOfWork.Notifications
            .GetUnreadCountByUserIdAsync(
                userId,
                cancellationToken);
    }


    // ============================================================
    // CREATE
    // ============================================================

    public async Task<NotificationResponseDto> CreateAsync(
        CreateNotificationRequestDto request,
        CancellationToken cancellationToken = default)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        ValidateUserId(request.UserId);

        var user =
            await _unitOfWork.Users.GetByIdAsync(
                request.UserId,
                cancellationToken);

        if (user is null)
            throw new KeyNotFoundException(
                "The notification recipient was not found.");

        var notification =
            Notification.Create(
                request.UserId,
                request.Title,
                request.Message,
                request.Type,
                request.Priority,
                request.ReferenceId,
                request.ReferenceType,
                request.ActionUrl,
                request.ExpiresAt);

        await _unitOfWork.Notifications.AddAsync(
            notification,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(notification);
    }


    // ============================================================
    // MARK AS READ
    // ============================================================

    public async Task<NotificationResponseDto> MarkAsReadAsync(
        MarkNotificationAsReadRequestDto request,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        ValidateUserId(userId);

        var notification =
            await _unitOfWork.Notifications.GetByIdForUserAsync(
                request.NotificationId,
                userId,
                cancellationToken);

        if (notification is null)
            throw new KeyNotFoundException(
                "Notification record was not found.");

        notification.MarkAsRead();

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(notification);
    }


    // ============================================================
    // VALIDATION
    // ============================================================

    private static void ValidateUserId(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new UnauthorizedAccessException(
                "The authenticated user identifier is missing or invalid.");
    }


    // ============================================================
    // MAP RESPONSE
    // ============================================================

    private static NotificationResponseDto MapToResponse(
        Notification notification)
    {
        return new NotificationResponseDto
        {
            Id = notification.Id,

            UserId = notification.UserId,

            Title = notification.Title,

            Message = notification.Message,

            Type = notification.Type,

            Priority = notification.Priority,

            Status = notification.Status,

            ReferenceId = notification.ReferenceId,

            ReferenceType = notification.ReferenceType,

            ActionUrl = notification.ActionUrl,

            ReadAt = notification.ReadAt,

            ExpiresAt = notification.ExpiresAt,

            IsActive = notification.IsActive,

            CreatedAt = notification.CreatedAt
        };
    }


    // ============================================================
    // MAP DETAIL RESPONSE
    // ============================================================

    private static NotificationDetailResponseDto
        MapToDetailResponse(Notification notification)
    {
        return new NotificationDetailResponseDto
        {
            Id = notification.Id,

            UserId = notification.UserId,

            UserName = notification.User?.Username,

            Title = notification.Title,

            Message = notification.Message,

            Type = notification.Type,

            Priority = notification.Priority,

            Status = notification.Status,

            ReferenceId = notification.ReferenceId,

            ReferenceType = notification.ReferenceType,

            ActionUrl = notification.ActionUrl,

            ReadAt = notification.ReadAt,

            ExpiresAt = notification.ExpiresAt,

            IsActive = notification.IsActive,

            IsDeleted = notification.IsDeleted,

            CreatedAt = notification.CreatedAt,

            CreatedBy = notification.CreatedBy,

            UpdatedAt = notification.UpdatedAt,

            UpdatedBy = notification.UpdatedBy
        };
    }
}