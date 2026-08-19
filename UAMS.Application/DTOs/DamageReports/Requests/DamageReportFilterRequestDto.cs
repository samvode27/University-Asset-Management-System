using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.DamageReports.Requests;

public class DamageReportFilterRequestDto
{
    public string? ReportNumber { get; set; }

    public Guid? AssetId { get; set; }

    public Guid? AssetAssignmentId { get; set; }

    public Guid? ReportedById { get; set; }

    public Guid? AssessedById { get; set; }

    public DamageType? DamageType { get; set; }

    public DamageSeverity? Severity { get; set; }

    public DamageReportStatus? Status { get; set; }

    public DateTime? ReportedFromDate { get; set; }

    public DateTime? ReportedToDate { get; set; }

    public bool? IsRepairable { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 20;

    public string? SearchTerm { get; set; }
}