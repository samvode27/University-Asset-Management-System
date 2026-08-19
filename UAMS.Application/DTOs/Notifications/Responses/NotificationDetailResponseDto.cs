                                                                                                    using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.Notifications.Responses;

public class NotificationDetailResponseDto
{
    // ============================================================
    // Identity
    // ============================================================

    public Guid Id { get; set; }


    // ============================================================
    // User
    // ============================================================

    public Guid UserId { get; set; }

    public string? UserName { get; set; }


    // ============================================================
    // Content
    // ============================================================

    public string Title { get; set; } = null!;

    public string Message { get; set; } = null!;


    // ============================================================
    // Classification
    // ============================================================

    public NotificationType Type { get; set; }

    public NotificationPriority Priority { get; set; }

    public NotificationStatus Status { get; set; }


    // ============================================================
    // Reference
    // ============================================================

    public Guid? ReferenceId { get; set; }

    public string? ReferenceType { get; set; }


    // ============================================================
    // Action
    // ============================================================

    public string? ActionUrl { get; set; }


    // ============================================================
    // Read Information
    // ============================================================

    public DateTime? ReadAt { get; set; }


    // ============================================================
    // Expiration
    // ============================================================

    public DateTime? ExpiresAt { get; set; }


    // ============================================================
    // State
    // ============================================================

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }


    // ============================================================
    // Audit
    // ============================================================

    public DateTime CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }
}