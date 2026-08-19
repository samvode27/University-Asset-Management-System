namespace UAMS.Application.DTOs.Reports.Responses;

public class DisposalReportResponseDto
{
    public Guid DisposalId { get; set; }

    public string DisposalNumber { get; set; } = null!;

    public string AssetTag { get; set; } = null!;

    public string AssetName { get; set; } = null!;

    public string Reason { get; set; } = null!;

    public string? DisposalMethod { get; set; }

    public decimal? BookValue { get; set; }

    public decimal? EstimatedValue { get; set; }

    public decimal? DisposalValue { get; set; }

    public DateTime RequestedDate { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public DateTime? DisposalDate { get; set; }

    public string Status { get; set; } = null!;
}