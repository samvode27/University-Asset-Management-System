using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.DamageReports.Responses;

public class DamageReportResponseDto
{
    public Guid Id { get; set; }

    public string ReportNumber { get; set; } = null!;

    public Guid AssetId { get; set; }

    public Guid AssetAssignmentId { get; set; }

    public Guid ReportedById { get; set; }

    public DateTime ReportedDate { get; set; }

    public DamageType DamageType { get; set; }

    public DamageSeverity Severity { get; set; }

    public string Description { get; set; } = null!;

    public DateTime? IncidentDate { get; set; }

    public string? IncidentLocation { get; set; }

    public bool? IsRepairable { get; set; }

    public string? Assessment { get; set; }

    public Guid? AssessedById { get; set; }

    public DateTime? AssessedDate { get; set; }

    public DamageReportStatus Status { get; set; }

    public string? ResolutionRemarks { get; set; }

    public DateTime? ResolvedDate { get; set; }

    public string? Remarks { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}