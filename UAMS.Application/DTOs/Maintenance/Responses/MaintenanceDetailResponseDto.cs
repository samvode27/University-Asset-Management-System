using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.Maintenance.Responses;

public class MaintenanceDetailResponseDto
{
    public Guid Id { get; set; }

    public string MaintenanceNumber { get; set; } = null!;


    // ============================================================
    // Asset
    // ============================================================

    public Guid AssetId { get; set; }

    public string? AssetTag { get; set; }

    public string? AssetName { get; set; }

    public string? AssetSerialNumber { get; set; }


    // ============================================================
    // Damage Report
    // ============================================================

    public Guid? DamageReportId { get; set; }

    public string? DamageReportNumber { get; set; }


    // ============================================================
    // Requester
    // ============================================================

    public Guid RequestedById { get; set; }

    public string? RequestedByName { get; set; }


    // ============================================================
    // Technician
    // ============================================================

    public Guid? AssignedTechnicianId { get; set; }

    public string? AssignedTechnicianName { get; set; }


    // ============================================================
    // Maintenance Information
    // ============================================================

    public MaintenanceType MaintenanceType { get; set; }

    public string ProblemDescription { get; set; } = null!;

    public string? MaintenanceDescription { get; set; }

    public string? PartsUsed { get; set; }


    // ============================================================
    // Cost
    // ============================================================

    public decimal? EstimatedCost { get; set; }

    public decimal? ActualCost { get; set; }


    // ============================================================
    // Dates
    // ============================================================

    public DateTime RequestedDate { get; set; }

    public DateTime? StartedDate { get; set; }

    public DateTime? CompletedDate { get; set; }


    // ============================================================
    // Result
    // ============================================================

    public MaintenanceResult? Result { get; set; }

    public string? FailureReason { get; set; }


    // ============================================================
    // Status
    // ============================================================

    public MaintenanceStatus Status { get; set; }

    public bool IsActive { get; set; }


    // ============================================================
    // Remarks
    // ============================================================

    public string? Remarks { get; set; }


    // ============================================================
    // Audit Information
    // ============================================================

    public DateTime CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public Guid? ModifiedBy { get; set; }
}