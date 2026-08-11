using UAMS.Domain.Common;
using UAMS.Domain.Entities.AssetRequests;
using UAMS.Domain.Entities.Assets;
using UAMS.Domain.Entities.Users;
using UAMS.Domain.Enums;

namespace UAMS.Domain.Entities.AssetAssignments;

public class AssetAssignment : AuditableEntity
{
    private AssetAssignment()
    {
    }

    public string AssignmentNumber { get; private set; } = null!;

    public Guid AssetId { get; private set; }

    public Guid AssetRequestId { get; private set; }

    public Guid EmployeeId { get; private set; }

    public Guid AssignedById { get; private set; }

    public DateTime AssignedDate { get; private set; }

    public DateTime? ExpectedReturnDate { get; private set; }

    public DateTime? ActualReturnDate { get; private set; }

    public string? AssignmentLocation { get; private set; }

    public AssetCondition ConditionAtAssignment { get; private set; }

    public string? Remarks { get; private set; }

    public AssetAssignmentStatus Status { get; private set; }

    public Asset Asset { get; private set; } = null!;

    public AssetRequest AssetRequest { get; private set; } = null!;

    public User Employee { get; private set; } = null!;

    public User AssignedBy { get; private set; } = null!;



}