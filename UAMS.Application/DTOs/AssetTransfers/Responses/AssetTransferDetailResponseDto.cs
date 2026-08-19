using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.AssetTransfers.Responses;

public class AssetTransferDetailResponseDto
{
    public Guid Id { get; set; }

    public string TransferNumber { get; set; } = null!;


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

    public string? AssignmentNumber { get; set; }


    // ============================================================
    // Request
    // ============================================================

    public Guid RequestedById { get; set; }

    public string RequestedByName { get; set; } = null!;

    public DateTime RequestedDate { get; set; }


    // ============================================================
    // Current Employee / Department
    // ============================================================

    public Guid FromEmployeeId { get; set; }

    public string FromEmployeeName { get; set; } = null!;

    public Guid FromDepartmentId { get; set; }

    public string FromDepartmentName { get; set; } = null!;

    public string? FromLocation { get; set; }


    // ============================================================
    // Destination
    // ============================================================

    public Guid ToEmployeeId { get; set; }

    public string ToEmployeeName { get; set; } = null!;

    public Guid ToDepartmentId { get; set; }

    public string ToDepartmentName { get; set; } = null!;

    public string? ToLocation { get; set; }


    // ============================================================
    // Transfer Information
    // ============================================================

    public string Reason { get; set; } = null!;

    public string? Remarks { get; set; }


    // ============================================================
    // Approval
    // ============================================================

    public Guid? ApprovedById { get; set; }

    public string? ApprovedByName { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public string? ApprovalRemarks { get; set; }


    // ============================================================
    // Completion
    // ============================================================

    public DateTime? CompletedDate { get; set; }


    // ============================================================
    // Status
    // ============================================================

    public AssetTransferStatus Status { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}