using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.AssetTransfers.Requests;

public class AssetTransferFilterRequestDto
{
    public string? TransferNumber { get; set; }

    public Guid? AssetId { get; set; }

    public Guid? AssetAssignmentId { get; set; }

    public Guid? RequestedById { get; set; }

    public Guid? FromEmployeeId { get; set; }

    public Guid? ToEmployeeId { get; set; }

    public Guid? FromDepartmentId { get; set; }

    public Guid? ToDepartmentId { get; set; }

    public AssetTransferStatus? Status { get; set; }

    public DateTime? RequestedDateFrom { get; set; }

    public DateTime? RequestedDateTo { get; set; }

    public DateTime? CompletedDateFrom { get; set; }

    public DateTime? CompletedDateTo { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}