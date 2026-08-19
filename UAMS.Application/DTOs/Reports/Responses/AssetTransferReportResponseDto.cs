namespace UAMS.Application.DTOs.Reports.Responses;

public class AssetTransferReportResponseDto
{
    public Guid TransferId { get; set; }

    public string TransferNumber { get; set; } = null!;

    public string AssetTag { get; set; } = null!;

    public string AssetName { get; set; } = null!;

    public string FromEmployee { get; set; } = null!;

    public string ToEmployee { get; set; } = null!;

    public string FromDepartment { get; set; } = null!;

    public string ToDepartment { get; set; } = null!;

    public string Reason { get; set; } = null!;

    public DateTime RequestedDate { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public DateTime? CompletedDate { get; set; }

    public string Status { get; set; } = null!;
}