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


    // ============================================================
    // Properties
    // ============================================================

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


    // ============================================================
    // Navigation Properties
    // ============================================================

    public Asset Asset { get; private set; } = null!;

    public AssetAssignment AssetAssignment { get; private set; } = null!;

    public User ReportedBy { get; private set; } = null!;

    public User? AssessedBy { get; private set; }


    // ============================================================
    // Factory
    // ============================================================

    public static DamageReport Create(
        string reportNumber,
        Guid assetId,
        Guid assetAssignmentId,
        Guid reportedById,
        DamageType damageType,
        DamageSeverity severity,
        string description,
        DateTime? incidentDate = null,
        string? incidentLocation = null,
        string? remarks = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            reportNumber,
            nameof(reportNumber));

        if (assetId == Guid.Empty)
        {
            throw new ArgumentException(
                "Asset ID is required.",
                nameof(assetId));
        }

        if (assetAssignmentId == Guid.Empty)
        {
            throw new ArgumentException(
                "Asset assignment ID is required.",
                nameof(assetAssignmentId));
        }

        if (reportedById == Guid.Empty)
        {
            throw new ArgumentException(
                "Reported by user ID is required.",
                nameof(reportedById));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(
            description,
            nameof(description));

        if (incidentDate.HasValue &&
            incidentDate.Value > DateTime.UtcNow)
        {
            throw new ArgumentException(
                "Incident date cannot be in the future.",
                nameof(incidentDate));
        }

        return new DamageReport
        {
            Id = Guid.NewGuid(),

            ReportNumber = reportNumber.Trim(),

            AssetId = assetId,

            AssetAssignmentId = assetAssignmentId,

            ReportedById = reportedById,

            ReportedDate = DateTime.UtcNow,

            DamageType = damageType,

            Severity = severity,

            Description = description.Trim(),

            IncidentDate = incidentDate,

            IncidentLocation = Normalize(incidentLocation),

            Remarks = Normalize(remarks),

            IsRepairable = null,

            Assessment = null,

            AssessedById = null,

            AssessedDate = null,

            Status = DamageReportStatus.Submitted,

            ResolutionRemarks = null,

            ResolvedDate = null,

            IsActive = true,

            IsDeleted = false
        };
    }


    // ============================================================
    // Update
    // ============================================================

    public void Update(
        DamageType damageType,
        DamageSeverity severity,
        string description,
        DateTime? incidentDate,
        string? incidentLocation,
        string? remarks)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            description,
            nameof(description));

        if (incidentDate.HasValue &&
            incidentDate.Value > DateTime.UtcNow)
        {
            throw new ArgumentException(
                "Incident date cannot be in the future.",
                nameof(incidentDate));
        }

        DamageType = damageType;

        Severity = severity;

        Description = description.Trim();

        IncidentDate = incidentDate;

        IncidentLocation = Normalize(incidentLocation);

        Remarks = Normalize(remarks);
    }


    // ============================================================
    // Start Review
    // ============================================================

    public void StartReview()
    {
        Status = DamageReportStatus.UnderReview;
    }


    // ============================================================
    // Mark Maintenance Required
    // ============================================================

    public void MarkMaintenanceRequired(
        Guid assessedById,
        string assessment)
    {
        if (assessedById == Guid.Empty)
        {
            throw new ArgumentException(
                "Assessed by user ID is required.",
                nameof(assessedById));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(
            assessment,
            nameof(assessment));

        AssessedById = assessedById;

        AssessedDate = DateTime.UtcNow;

        Assessment = assessment.Trim();

        IsRepairable = true;

        Status = DamageReportStatus.MaintenanceRequired;
    }


    // ============================================================
    // Mark Unrepairable
    // ============================================================

    public void MarkUnrepairable(
        Guid assessedById,
        string assessment)
    {
        if (assessedById == Guid.Empty)
        {
            throw new ArgumentException(
                "Assessed by user ID is required.",
                nameof(assessedById));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(
            assessment,
            nameof(assessment));

        AssessedById = assessedById;

        AssessedDate = DateTime.UtcNow;

        Assessment = assessment.Trim();

        IsRepairable = false;

        Status = DamageReportStatus.Unrepairable;
    }


    // ============================================================
    // Resolve
    // ============================================================

    public void Resolve(string? resolutionRemarks)
    {
        ResolutionRemarks = Normalize(resolutionRemarks);

        ResolvedDate = DateTime.UtcNow;

        Status = DamageReportStatus.Resolved;

        IsActive = true;
    }


    // ============================================================
    // Reject
    // ============================================================

    public void Reject(
        Guid assessedById,
        string rejectionReason)
    {
        if (assessedById == Guid.Empty)
        {
            throw new ArgumentException(
                "Assessed by user ID is required.",
                nameof(assessedById));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(
            rejectionReason,
            nameof(rejectionReason));

        AssessedById = assessedById;

        AssessedDate = DateTime.UtcNow;

        Assessment = rejectionReason.Trim();

        Status = DamageReportStatus.Rejected;

        IsActive = false;
    }


    // ============================================================
    // Cancel
    // ============================================================

    public void Cancel()
    {
        Status = DamageReportStatus.Cancelled;

        IsActive = false;
    }


    // ============================================================
    // Activate
    // ============================================================

    public void Activate()
    {
        Status = DamageReportStatus.Submitted;

        IsActive = true;

        IsDeleted = false;
    }


    // ============================================================
    // Soft Delete
    // ============================================================

    public void MarkDeleted(Guid deletedBy)
    {
        if (deletedBy == Guid.Empty)
        {
            throw new ArgumentException(
                "Deleted by user ID is required.",
                nameof(deletedBy));
        }

        IsDeleted = true;

        IsActive = false;

        DeletedAt = DateTime.UtcNow;

        DeletedBy = deletedBy;
    }


    // ============================================================
    // Private Helpers
    // ============================================================

    private static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}