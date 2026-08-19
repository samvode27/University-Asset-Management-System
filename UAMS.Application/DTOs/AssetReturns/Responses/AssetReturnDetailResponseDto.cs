using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.AssetReturns.Responses;

public class AssetReturnDetailResponseDto
{
    public Guid Id { get; set; }

    public string ReturnNumber { get; set; } = null!;

    // ============================================================
    // Asset
    // ============================================================

    public Guid AssetId { get; set; }

    public string AssetTag { get; set; } = null!;

    public string AssetName { get; set; } = null!;

    // ============================================================
    // Assignment
    // ============================================================

    public Guid AssetAssignmentId { get; set; }

    public string AssignmentNumber { get; set; } = null!;

    // ============================================================
    // Return
    // ============================================================

    public Guid ReturnedById { get; set; }

    public string ReturnedByName { get; set; } = null!;

    public Guid ReceivedById { get; set; }

    public string ReceivedByName { get; set; } = null!;

    public DateTime ReturnDate { get; set; }

    public string? ReturnLocation { get; set; }

    public AssetReturnCondition Condition { get; set; }

    // ============================================================
    // Inspection
    // ============================================================

    public Guid? InspectedById { get; set; }

    public string? InspectedByName { get; set; }

    public DateTime? InspectionDate { get; set; }

    public string? InspectionNotes { get; set; }

    // ============================================================
    // Damage
    // ============================================================

    public bool DamageFound { get; set; }

    public Guid? DamageReportId { get; set; }

    public string? DamageReportNumber { get; set; }

    // ============================================================
    // Status
    // ============================================================

    public AssetReturnStatus Status { get; set; }

    public string? Remarks { get; set; }

    // ============================================================
    // Audit
    // ============================================================

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}