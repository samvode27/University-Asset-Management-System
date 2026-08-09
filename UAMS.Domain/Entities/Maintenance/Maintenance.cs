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

    public Maintenance(
        string maintenanceNumber,
        Guid assetId,
        Guid? damageReportId,
        Guid requestedById,
        MaintenanceType maintenanceType,
        string problemDescription,
        DateTime requestedDate,
        string? remarks)
    {
        MaintenanceNumber = maintenanceNumber;
        AssetId = assetId;
        DamageReportId = damageReportId;
        RequestedById = requestedById;
        MaintenanceType = maintenanceType;
        ProblemDescription = problemDescription;
        RequestedDate = requestedDate;
        Remarks = remarks;

        Status = MaintenanceStatus.Pending;
        Result = null;
        IsActive = true;
    }

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

    public bool IsActive { get; private set; }

    // Navigation Properties
    public Asset Asset { get; private set; } = null!;

    public DamageReport? DamageReport { get; private set; }

    public User RequestedBy { get; private set; } = null!;

    public User? AssignedTechnician { get; private set; }

    public void Update(
        MaintenanceType maintenanceType,
        string problemDescription,
        decimal? estimatedCost,
        string? remarks)
    {
        MaintenanceType = maintenanceType;
        ProblemDescription = problemDescription;
        EstimatedCost = estimatedCost;
        Remarks = remarks;
    }

    public void AssignTechnician(Guid technicianId)
    {
        AssignedTechnicianId = technicianId;
        Status = MaintenanceStatus.Approved;
    }

    public void Start()
    {
        StartedDate = DateTime.UtcNow;
        Status = MaintenanceStatus.InProgress;
    }

    public void CompleteAsRepaired(
        decimal actualCost,
        string? maintenanceDescription,
        string? partsUsed,
        string? remarks)
    {
        ActualCost = actualCost;
        MaintenanceDescription = maintenanceDescription;
        PartsUsed = partsUsed;
        Remarks = remarks;

        Result = MaintenanceResult.Repaired;
        CompletedDate = DateTime.UtcNow;
        Status = MaintenanceStatus.Completed;
    }

    public void CompleteAsUnrepairable(
        decimal actualCost,
        string failureReason,
        string? maintenanceDescription,
        string? partsUsed,
        string? remarks)
    {
        ActualCost = actualCost;
        FailureReason = failureReason;
        MaintenanceDescription = maintenanceDescription;
        PartsUsed = partsUsed;
        Remarks = remarks;

        Result = MaintenanceResult.Unrepairable;
        CompletedDate = DateTime.UtcNow;
        Status = MaintenanceStatus.Failed;
    }

    public void CompleteAsPartiallyRepaired(
        decimal actualCost,
        string? maintenanceDescription,
        string? partsUsed,
        string? remarks)
    {
        ActualCost = actualCost;
        MaintenanceDescription = maintenanceDescription;
        PartsUsed = partsUsed;
        Remarks = remarks;

        Result = MaintenanceResult.PartiallyRepaired;
        CompletedDate = DateTime.UtcNow;
        Status = MaintenanceStatus.Completed;
    }

    public void Cancel()
    {
        Status = MaintenanceStatus.Cancelled;
        IsActive = false;
    }

    public void Activate()
    {
        Status = MaintenanceStatus.Pending;
        IsActive = true;
    }

    public void MarkDeleted(Guid deletedBy)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
    }
}