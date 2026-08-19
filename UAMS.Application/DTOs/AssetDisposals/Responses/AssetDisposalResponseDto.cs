using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.AssetDisposals.Responses;

public class AssetDisposalResponseDto
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


    // ============================================================
    // Maintenance
    // ============================================================

    public Guid? MaintenanceId { get; set; }

    public string? MaintenanceNumber { get; set; }


    // ============================================================
    // Users
    // ============================================================

    public Guid RequestedById { get; set; }

    public string? RequestedByName { get; set; }

    public Guid? ApprovedById { get; set; }

    public string? ApprovedByName { get; set; }

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


    // ============================================================
    // Common State
    // ============================================================

    public bool IsActive { get; set; }
}