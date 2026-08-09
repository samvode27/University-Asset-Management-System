using UAMS.Domain.Common;
using UAMS.Domain.Entities.AssetAssignments;
using UAMS.Domain.Entities.Assets;
using UAMS.Domain.Entities.Users;
using UAMS.Domain.Enums;

namespace UAMS.Domain.Entities.DamageReports;

public class DamageReport : AuditableEntity
{
    private DamageReport()
    {
    }

    public DamageReport(
        string reportNumber,
        Guid assetId,
        Guid assetAssignmentId,
        Guid reportedById,
        DateTime reportedDate,
        DamageType damageType,
        DamageSeverity severity,
        string description,
        DateTime? incidentDate,
        string? incidentLocation,
        string? remarks)
    {
        ReportNumber = reportNumber;
        AssetId = assetId;
        AssetAssignmentId = assetAssignmentId;
        ReportedById = reportedById;
        ReportedDate = reportedDate;
        DamageType = damageType;
        Severity = severity;
        Description = description;
        IncidentDate = incidentDate;
        IncidentLocation = incidentLocation;
        Remarks = remarks;

        Status = DamageReportStatus.Submitted;
        IsRepairable = null;
        IsActive = true;
    }

    public string ReportNumber { get; private set; } = null!;

    public Guid AssetId { get; private set; }

    public Guid AssetAssignmentId { get; private set; }

    public Guid ReportedById { get; private set; }

    public DateTime ReportedDate { get; private set; }

    public DamageType DamageType { get; private set; }

    public DamageSeverity Severity { get; private set; }

    public string Description { get; private set; } = null!;

    public DateTime? IncidentDate { get; private set; }

    public string? IncidentLocation { get; private set; }

    public bool? IsRepairable { get; private set; }

    public string? Assessment { get; private set; }

    public Guid? AssessedById { get; private set; }

    public DateTime? AssessedDate { get; private set; }

    public DamageReportStatus Status { get; private set; }

    public string? ResolutionRemarks { get; private set; }

    public DateTime? ResolvedDate { get; private set; }

    public string? Remarks { get; private set; }

    public bool IsActive { get; private set; }

    // Navigation Properties
    public Asset Asset { get; private set; } = null!;

    public AssetAssignment AssetAssignment { get; private set; } = null!;

    public User ReportedBy { get; private set; } = null!;

    public User? AssessedBy { get; private set; }

    public void Update(
        DamageType damageType,
        DamageSeverity severity,
        string description,
        DateTime? incidentDate,
        string? incidentLocation,
        string? remarks)
    {
        DamageType = damageType;
        Severity = severity;
        Description = description;
        IncidentDate = incidentDate;
        IncidentLocation = incidentLocation;
        Remarks = remarks;
    }

    public void StartReview()
    {
        Status = DamageReportStatus.UnderReview;
    }

    public void MarkMaintenanceRequired(
        Guid assessedById,
        string assessment)
    {
        AssessedById = assessedById;
        AssessedDate = DateTime.UtcNow;
        Assessment = assessment;
        IsRepairable = true;

        Status = DamageReportStatus.MaintenanceRequired;
    }

    public void MarkUnrepairable(
        Guid assessedById,
        string assessment)
    {
        AssessedById = assessedById;
        AssessedDate = DateTime.UtcNow;
        Assessment = assessment;
        IsRepairable = false;

        Status = DamageReportStatus.Unrepairable;
    }

    public void Resolve(string? resolutionRemarks)
    {
        ResolutionRemarks = resolutionRemarks;
        ResolvedDate = DateTime.UtcNow;

        Status = DamageReportStatus.Resolved;
    }

    public void Reject(
        Guid assessedById,
        string rejectionReason)
    {
        AssessedById = assessedById;
        AssessedDate = DateTime.UtcNow;
        Assessment = rejectionReason;

        Status = DamageReportStatus.Rejected;
        IsActive = false;
    }

    public void Cancel()
    {
        Status = DamageReportStatus.Cancelled;
        IsActive = false;
    }

    public void Activate()
    {
        Status = DamageReportStatus.Submitted;
        IsActive = true;
    }

    public void MarkDeleted(Guid deletedBy)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
    }
}