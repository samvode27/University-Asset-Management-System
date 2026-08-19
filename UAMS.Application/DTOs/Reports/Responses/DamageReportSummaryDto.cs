namespace UAMS.Application.DTOs.Reports.Responses;

public class DamageReportSummaryDto
{
    public Guid DamageReportId { get; set; }

    public string ReportNumber { get; set; } = null!;

    public string AssetTag { get; set; } = null!;

    public string AssetName { get; set; } = null!;

    public string DamageType { get; set; } = null!;

    public string Severity { get; set; } = null!;

    public bool? IsRepairable { get; set; }

    public string Status { get; set; } = null!;

    public DateTime ReportedDate { get; set; }

    public DateTime? ResolvedDate { get; set; }
}