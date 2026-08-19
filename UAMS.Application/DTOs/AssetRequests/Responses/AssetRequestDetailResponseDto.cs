using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.AssetRequests.Responses;

public class AssetRequestDetailResponseDto
{
    // ============================================================
    // Request
    // ============================================================

    public Guid Id { get; set; }

    public string RequestNumber { get; set; } = null!;

    public string Purpose { get; set; } = null!;

    public DateTime RequestedDate { get; set; }

    public DateTime? RequiredFromDate { get; set; }

    public DateTime? RequiredToDate { get; set; }

    public AssetRequestStatus Status { get; set; }

    public bool IsActive { get; set; }

    // ============================================================
    // Asset
    // ============================================================

    public Guid AssetId { get; set; }

    public string? AssetTag { get; set; }

    public string? AssetName { get; set; }

    public string? SerialNumber { get; set; }

    public string? AssetStatus { get; set; }

    // ============================================================
    // Requester
    // ============================================================

    public Guid RequesterId { get; set; }

    public string? RequesterName { get; set; }

    public string? RequesterEmail { get; set; }

    // ============================================================
    // Department
    // ============================================================

    public Guid DepartmentId { get; set; }

    public string? DepartmentName { get; set; }

    // ============================================================
    // Department Head Approval
    // ============================================================

    public Guid? DepartmentHeadId { get; set; }

    public string? DepartmentHeadName { get; set; }

    public DateTime? DepartmentHeadActionDate { get; set; }

    public string? DepartmentHeadRemarks { get; set; }

    // ============================================================
    // Asset Manager Approval
    // ============================================================

    public Guid? AssetManagerId { get; set; }

    public string? AssetManagerName { get; set; }

    public DateTime? AssetManagerActionDate { get; set; }

    public string? AssetManagerRemarks { get; set; }

    // ============================================================
    // Rejection
    // ============================================================

    public string? RejectionReason { get; set; }

    // ============================================================
    // Audit
    // ============================================================

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}