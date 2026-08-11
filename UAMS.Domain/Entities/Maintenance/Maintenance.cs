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

    // Navigation Properties
    public Asset Asset { get; private set; } = null!;

    public DamageReport? DamageReport { get; private set; }

    public User RequestedBy { get; private set; } = null!;

    public User? AssignedTechnician { get; private set; }

}