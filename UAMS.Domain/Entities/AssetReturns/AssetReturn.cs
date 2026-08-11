using UAMS.Domain.Common;
using UAMS.Domain.Entities.AssetAssignments;
using UAMS.Domain.Entities.Assets;
using UAMS.Domain.Entities.DamageReports;
using UAMS.Domain.Entities.Users;
using UAMS.Domain.Enums;

namespace UAMS.Domain.Entities.AssetReturns;

public class AssetReturn : AuditableEntity
{
    private AssetReturn()
    {
    }

    public string ReturnNumber { get; private set; } = null!;

    public Guid AssetId { get; private set; }

    public Guid AssetAssignmentId { get; private set; }

    public Guid ReturnedById { get; private set; }

    public Guid ReceivedById { get; private set; }

    public DateTime ReturnDate { get; private set; }

    public string? ReturnLocation { get; private set; }

    public AssetReturnCondition Condition { get; private set; }

    public string? InspectionNotes { get; private set; }

    public Guid? InspectedById { get; private set; }

    public DateTime? InspectionDate { get; private set; }

    public bool DamageFound { get; private set; }

    public Guid? DamageReportId { get; private set; }

    public string? Remarks { get; private set; }

    public AssetReturnStatus Status { get; private set; }

    // Navigation Properties
    public Asset Asset { get; private set; } = null!;

    public AssetAssignment AssetAssignment { get; private set; } = null!;

    public User ReturnedBy { get; private set; } = null!;

    public User ReceivedBy { get; private set; } = null!;

    public User? InspectedBy { get; private set; }

    public DamageReport? DamageReport { get; private set; }

    
}