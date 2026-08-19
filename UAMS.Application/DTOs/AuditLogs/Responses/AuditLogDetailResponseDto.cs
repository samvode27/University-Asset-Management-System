using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.AuditLogs.Responses;

public class AuditLogDetailResponseDto
{
    // ============================================================
    // Identity
    // ============================================================

    public Guid Id { get; set; }


    // ============================================================
    // User
    // ============================================================

    public Guid? UserId { get; set; }

    public string? UserName { get; set; }


    // ============================================================
    // Action
    // ============================================================

    public AuditAction Action { get; set; }


    // ============================================================
    // Entity
    // ============================================================

    public string EntityName { get; set; } = null!;

    public Guid? EntityId { get; set; }


    // ============================================================
    // Description
    // ============================================================

    public string Description { get; set; } = null!;


    // ============================================================
    // Change Information
    // ============================================================

    public string? OldValues { get; set; }

    public string? NewValues { get; set; }

    public string? ChangedProperties { get; set; }


    // ============================================================
    // Request Information
    // ============================================================

    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }

    public string? RequestId { get; set; }


    // ============================================================
    // Severity
    // ============================================================

    public AuditSeverity Severity { get; set; }


    // ============================================================
    // Timestamp
    // ============================================================

    public DateTime Timestamp { get; set; }


    // ============================================================
    // Result
    // ============================================================

    public bool IsSuccessful { get; set; }

    public string? ErrorMessage { get; set; }


    // ============================================================
    // Audit Entity Information
    // ============================================================

    public DateTime CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }
}