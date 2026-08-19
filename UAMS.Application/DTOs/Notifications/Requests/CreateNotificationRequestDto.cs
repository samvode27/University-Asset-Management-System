using System.ComponentModel.DataAnnotations;
using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.Notifications.Requests;

public class CreateNotificationRequestDto
{
    // ============================================================
    // Recipient
    // ============================================================

    [Required]
    public Guid UserId { get; set; }


    // ============================================================
    // Notification Content
    // ============================================================

    [Required]
    [MaxLength(250)]
    public string Title { get; set; } = null!;


    [Required]
    [MaxLength(2000)]
    public string Message { get; set; } = null!;


    // ============================================================
    // Classification
    // ============================================================

    [Required]
    public NotificationType Type { get; set; }


    [Required]
    public NotificationPriority Priority { get; set; }


    // ============================================================
    // Reference
    // ============================================================

    public Guid? ReferenceId { get; set; }


    [MaxLength(100)]
    public string? ReferenceType { get; set; }


    // ============================================================
    // Action
    // ============================================================

    [MaxLength(500)]
    public string? ActionUrl { get; set; }


    // ============================================================
    // Expiration
    // ============================================================

    public DateTime? ExpiresAt { get; set; }
}