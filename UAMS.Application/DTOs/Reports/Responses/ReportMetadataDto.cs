namespace UAMS.Application.DTOs.Reports.Responses;

public class ReportMetadataDto
{
    public string ReportName { get; set; } = null!;

    public DateTime GeneratedAt { get; set; }

    public string? GeneratedBy { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public string? DepartmentName { get; set; }

    public int RecordCount { get; set; }
}