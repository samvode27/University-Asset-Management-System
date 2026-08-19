using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.Notifications.Responses;

public class NotificationResponseDto
{
    // ============================================================
    // Identity
    // ============================================================

    public Guid Id { get; set; }


    // ============================================================
    // Recipient
    // ============================================================

    public Guid UserId { get; set; }


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
    // Common State
    // ============================================================

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}