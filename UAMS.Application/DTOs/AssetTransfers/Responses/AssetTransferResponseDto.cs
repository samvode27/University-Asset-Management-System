using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.AssetTransfers.Responses;

public class AssetTransferResponseDto
{
    public Guid Id { get; set; }

    public string TransferNumber { get; set; } = null!;

    public Guid AssetId { get; set; }

    public string AssetTag { get; set; } = null!;

    public string AssetName { get; set; } = null!;

    public Guid AssetAssignmentId { get; set; }

    public Guid RequestedById { get; set; }

    public string RequestedByName { get; set; } = null!;

    public Guid FromEmployeeId { get; set; }

    public string FromEmployeeName { get; set; } = null!;

    public Guid ToEmployeeId { get; set; }

    public string ToEmployeeName { get; set; } = null!;

    public Guid FromDepartmentId { get; set; }

    public string FromDepartmentName { get; set; } = null!;

    public Guid ToDepartmentId { get; set; }

    public string ToDepartmentName { get; set; } = null!;

    public string? FromLocation { get; set; }

    public string? ToLocation { get; set; }

    public string Reason { get; set; } = null!;

    public DateTime RequestedDate { get; set; }

    public Guid? ApprovedById { get; set; }

    public string? ApprovedByName { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public string? ApprovalRemarks { get; set; }

    public DateTime? CompletedDate { get; set; }

    public string? Remarks { get; set; }

    public AssetTransferStatus Status { get; set; }

    public bool IsActive { get; set; }
}