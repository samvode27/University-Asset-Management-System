using UAMS.Domain.Common;
using UAMS.Domain.Entities.Assets;
using UAMS.Domain.Entities.DamageReports;
using UAMS.Domain.Entities.Users;
using UAMS.Domain.Enums;

namespace UAMS.Domain.Entities.Maintenances;

public class Maintenance : AuditableEntity
{
    private Maintenance()
    {
    }


    // ============================================================
    // Properties
    // ============================================================

    public string MaintenanceNumber { get; private set; } = null!;

    public Guid AssetId { get; private set; }

    public Guid? DamageReportId { get; private set; }

    public Guid RequestedById { get; private set; }

    public Guid? AssignedTechnicianId { get; private set; }

    public MaintenanceType MaintenanceType { get; private set; }

    public string ProblemDescription { get; private set; } = null!;

    public string? MaintenanceDescription { get; private set; }

    public string? PartsUsed { get; private set; }

    public decimal? EstimatedCost { get; private set; }

    public decimal? ActualCost { get; private set; }

    public DateTime RequestedDate { get; private set; }

    public DateTime? StartedDate { get; private set; }

    public DateTime? CompletedDate { get; private set; }

    public MaintenanceResult? Result { get; private set; }

    public string? FailureReason { get; private set; }

    public string? Remarks { get; private set; }

    public MaintenanceStatus Status { get; private set; }


    // ============================================================
    // Navigation Properties
    // ============================================================

    public Asset Asset { get; private set; } = null!;

    public DamageReport? DamageReport { get; private set; }

    public User RequestedBy { get; private set; } = null!;

    public User? AssignedTechnician { get; private set; }


    // ============================================================
    // Factory
    // ============================================================

    public static Maintenance Create(
        string maintenanceNumber,
        Guid assetId,
        Guid? damageReportId,
        Guid requestedById,
        MaintenanceType maintenanceType,
        string problemDescription,
        string? maintenanceDescription = null,
        string? partsUsed = null,
        decimal? estimatedCost = null,
        DateTime? requestedDate = null,
        string? remarks = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            maintenanceNumber,
            nameof(maintenanceNumber));

        if (assetId == Guid.Empty)
        {
            throw new ArgumentException(
                "Asset ID is required.",
                nameof(assetId));
        }

        if (damageReportId.HasValue &&
            damageReportId.Value == Guid.Empty)
        {
            throw new ArgumentException(
                "Damage report ID must be valid when provided.",
                nameof(damageReportId));
        }

        if (requestedById == Guid.Empty)
        {
            throw new ArgumentException(
                "Requested by user ID is required.",
                nameof(requestedById));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(
            problemDescription,
            nameof(problemDescription));

        if (estimatedCost.HasValue &&
            estimatedCost.Value < 0)
        {
            throw new ArgumentException(
                "Estimated cost cannot be negative.",
                nameof(estimatedCost));
        }

        var effectiveRequestedDate =
            requestedDate ?? DateTime.UtcNow;

        if (effectiveRequestedDate > DateTime.UtcNow)
        {
            throw new ArgumentException(
                "Requested date cannot be in the future.",
                nameof(requestedDate));
        }

        return new Maintenance
        {
            Id = Guid.NewGuid(),

            MaintenanceNumber =
                maintenanceNumber.Trim(),

            AssetId =
                assetId,

            DamageReportId =
                damageReportId,

            RequestedById =
                requestedById,

            MaintenanceType =
                maintenanceType,

            ProblemDescription =
                problemDescription.Trim(),

            MaintenanceDescription =
                Normalize(maintenanceDescription),

            PartsUsed =
                Normalize(partsUsed),

            EstimatedCost =
                estimatedCost,

            ActualCost =
                null,

            RequestedDate =
                effectiveRequestedDate,

            StartedDate =
                null,

            CompletedDate =
                null,

            Result =
                null,

            FailureReason =
                null,

            Remarks =
                Normalize(remarks),

            Status =
                MaintenanceStatus.Pending,

            AssignedTechnicianId =
                null,

            IsActive =
                true,

            IsDeleted =
                false
        };
    }


    // ============================================================
    // Update
    // ============================================================

    public void Update(
        MaintenanceType maintenanceType,
        string problemDescription,
        string? maintenanceDescription,
        string? partsUsed,
        decimal? estimatedCost,
        string? remarks)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            problemDescription,
            nameof(problemDescription));

        if (estimatedCost.HasValue &&
            estimatedCost.Value < 0)
        {
            throw new ArgumentException(
                "Estimated cost cannot be negative.",
                nameof(estimatedCost));
        }

        MaintenanceType =
            maintenanceType;

        ProblemDescription =
            problemDescription.Trim();

        MaintenanceDescription =
            Normalize(maintenanceDescription);

        PartsUsed =
            Normalize(partsUsed);

        EstimatedCost =
            estimatedCost;

        Remarks =
            Normalize(remarks);
    }


    // ============================================================
    // Assign Technician
    // ============================================================

    public void AssignTechnician(
        Guid assignedTechnicianId,
        string? remarks = null)
    {
        if (assignedTechnicianId == Guid.Empty)
        {
            throw new ArgumentException(
                "Assigned technician ID is required.",
                nameof(assignedTechnicianId));
        }

        AssignedTechnicianId =
            assignedTechnicianId;

        if (!string.IsNullOrWhiteSpace(remarks))
        {
            Remarks =
                Normalize(remarks);
        }

        if (Status == MaintenanceStatus.Pending)
        {
            Status =
                MaintenanceStatus.Approved;
        }
    }


    // ============================================================
    // Start Maintenance
    // ============================================================

    public void Start(
        string? maintenanceDescription = null,
        string? partsUsed = null,
        string? remarks = null)
    {
        if (!AssignedTechnicianId.HasValue ||
            AssignedTechnicianId.Value == Guid.Empty)
        {
            throw new InvalidOperationException(
                "A technician must be assigned before maintenance can start.");
        }

        if (Status != MaintenanceStatus.Pending &&
            Status != MaintenanceStatus.Approved)
        {
            throw new InvalidOperationException(
                "Maintenance cannot be started in its current status.");
        }

        if (!string.IsNullOrWhiteSpace(maintenanceDescription))
        {
            MaintenanceDescription =
                Normalize(maintenanceDescription);
        }

        if (!string.IsNullOrWhiteSpace(partsUsed))
        {
            PartsUsed =
                Normalize(partsUsed);
        }

        if (!string.IsNullOrWhiteSpace(remarks))
        {
            Remarks =
                Normalize(remarks);
        }

        StartedDate =
            DateTime.UtcNow;

        Status =
            MaintenanceStatus.InProgress;
    }


    // ============================================================
    // Complete Maintenance
    // ============================================================

    public void Complete(
        MaintenanceResult result,
        decimal actualCost,
        string? maintenanceDescription,
        string? partsUsed,
        string? failureReason,
        string? remarks)
    {
        if (Status != MaintenanceStatus.InProgress)
        {
            throw new InvalidOperationException(
                "Only maintenance in progress can be completed.");
        }

        if (actualCost < 0)
        {
            throw new ArgumentException(
                "Actual cost cannot be negative.",
                nameof(actualCost));
        }

        MaintenanceDescription =
            Normalize(maintenanceDescription);

        PartsUsed =
            Normalize(partsUsed);

        FailureReason =
            Normalize(failureReason);

        Remarks =
            Normalize(remarks);

        ActualCost =
            actualCost;

        Result =
            result;

        CompletedDate =
            DateTime.UtcNow;

        Status =
            MaintenanceStatus.Completed;

        IsActive =
            true;
    }


    // ============================================================
    // Cancel
    // ============================================================

    public void Cancel(
        string reason)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            reason,
            nameof(reason));

        if (Status == MaintenanceStatus.Completed)
        {
            throw new InvalidOperationException(
                "Completed maintenance cannot be cancelled.");
        }

        Remarks =
            Normalize(reason);

        Status =
            MaintenanceStatus.Cancelled;

        IsActive =
            false;
    }


    // ============================================================
    // Activate
    // ============================================================

    public void Activate()
    {
        Status =
            MaintenanceStatus.Pending;

        IsActive =
            true;

        IsDeleted =
            false;
    }


    // ============================================================
    // Soft Delete
    // ============================================================

    public void MarkDeleted(
        Guid deletedBy)
    {
        if (deletedBy == Guid.Empty)
        {
            throw new ArgumentException(
                "Deleted by user ID is required.",
                nameof(deletedBy));
        }

        IsDeleted =
            true;

        IsActive =
            false;

        DeletedAt =
            DateTime.UtcNow;

        DeletedBy =
            deletedBy;
    }


    // ============================================================
    // Private Helpers
    // ============================================================

    private static string? Normalize(
        string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}

