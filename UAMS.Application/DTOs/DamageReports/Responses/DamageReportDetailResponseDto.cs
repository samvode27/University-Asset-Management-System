using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.DamageReports.Responses;

public class DamageReportDetailResponseDto
{
    public Guid Id { get; set; }

    public string ReportNumber { get; set; } = null!;

    // ============================================================
    // Asset
    // ============================================================

    public Guid AssetId { get; set; }

    public string? AssetTag { get; set; }

    public string? AssetName { get; set; }

    // ============================================================
    // Assignment
    // ============================================================

    public Guid AssetAssignmentId { get; set; }

    public string? AssignmentNumber { get; set; }

    // ============================================================
    // Reporter
    // ============================================================

    public Guid ReportedById { get; set; }

    public string? ReportedByName { get; set; }

    // ============================================================
    // Damage Information
    // ============================================================

    public DateTime ReportedDate { get; set; }

    public DamageType DamageType { get; set; }

    public DamageSeverity Severity { get; set; }

    public string Description { get; set; } = null!;

    public DateTime? IncidentDate { get; set; }

    public string? IncidentLocation { get; set; }

    // ============================================================
    // Assessment
    // ============================================================

    public bool? IsRepairable { get; set; }

    public string? Assessment { get; set; }

    public Guid? AssessedById { get; set; }

    public string? AssessedByName { get; set; }

    public DateTime? AssessedDate { get; set; }

    // ============================================================
    // Resolution
    // ============================================================

    public DamageReportStatus Status { get; set; }

    public string? ResolutionRemarks { get; set; }

    public DateTime? ResolvedDate { get; set; }

    public string? Remarks { get; set; }

    // ============================================================
    // Audit / State
    // ============================================================

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}