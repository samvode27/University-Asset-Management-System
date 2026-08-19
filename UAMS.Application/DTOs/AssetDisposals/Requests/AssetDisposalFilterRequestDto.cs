using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.AssetDisposals.Requests;

public class AssetDisposalFilterRequestDto
{
    // ============================================================
    // Search
    // ============================================================

    public string? SearchTerm { get; set; }


    // ============================================================
    // Asset
    // ============================================================

    public Guid? AssetId { get; set; }


    // ============================================================
    // Maintenance
    // ============================================================

    public Guid? MaintenanceId { get; set; }


    // ============================================================
    // Users
    // ============================================================

    public Guid? RequestedById { get; set; }

    public Guid? ApprovedById { get; set; }

    public Guid? CompletedById { get; set; }


    // ============================================================
    // Status
    // ============================================================

    public AssetDisposalStatus? Status { get; set; }


    // ============================================================
    // Disposal Method
    // ============================================================

    public DisposalMethod? DisposalMethod { get; set; }


    // ============================================================
    // Dates
    // ============================================================

    public DateTime? RequestedFromDate { get; set; }

    public DateTime? RequestedToDate { get; set; }

    public DateTime? ApprovedFromDate { get; set; }

    public DateTime? ApprovedToDate { get; set; }

    public DateTime? DisposalFromDate { get; set; }

    public DateTime? DisposalToDate { get; set; }


    // ============================================================
    // Pagination
    // ============================================================

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}