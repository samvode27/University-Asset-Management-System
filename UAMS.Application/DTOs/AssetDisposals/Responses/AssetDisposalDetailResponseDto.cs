using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.AssetDisposals.Responses;

public class AssetDisposalDetailResponseDto
{
    // ============================================================
    // Identity
    // ============================================================

    public Guid Id { get; set; }

    public string DisposalNumber { get; set; } = null!;


    // ============================================================
    // Asset
    // ============================================================

    public Guid AssetId { get; set; }

    public string? AssetTag { get; set; }

    public string? AssetName { get; set; }

    public string? AssetSerialNumber { get; set; }

    public string? AssetLocation { get; set; }


    // ============================================================
    // Maintenance
    // ============================================================

    public Guid? MaintenanceId { get; set; }

    public string? MaintenanceNumber { get; set; }


    // ============================================================
    // Requested By
    // ============================================================

    public Guid RequestedById { get; set; }

    public string? RequestedByName { get; set; }


    // ============================================================
    // Approved By
    // ============================================================

    public Guid? ApprovedById { get; set; }

    public string? ApprovedByName { get; set; }


    // ============================================================
    // Completed By
    // ============================================================

    public Guid? CompletedById { get; set; }

    public string? CompletedByName { get; set; }


    // ============================================================
    // Disposal Information
    // ============================================================

    public DisposalMethod? DisposalMethod { get; set; }

    public string Reason { get; set; } = null!;


    // ============================================================
    // Financial Information
    // ============================================================

    public decimal? BookValue { get; set; }

    public decimal? EstimatedValue { get; set; }

    public decimal? DisposalValue { get; set; }


    // ============================================================
    // Dates
    // ============================================================

    public DateTime RequestedDate { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public DateTime? DisposalDate { get; set; }


    // ============================================================
    // Remarks
    // ============================================================

    public string? Remarks { get; set; }


    // ============================================================
    // Status
    // ============================================================

    public AssetDisposalStatus Status { get; set; }

    public bool IsActive { get; set; }


    // ============================================================
    // Audit Information
    // ============================================================

    public DateTime CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }
}