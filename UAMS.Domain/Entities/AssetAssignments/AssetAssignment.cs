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

    public AssetAssignment(
        string assignmentNumber,
        Guid assetId,
        Guid assetRequestId,
        Guid employeeId,
        Guid assignedById,
        DateTime assignedDate,
        DateTime? expectedReturnDate,
        string? assignmentLocation,
        AssetCondition conditionAtAssignment,
        string? remarks)
    {
        AssignmentNumber = assignmentNumber;
        AssetId = assetId;
        AssetRequestId = assetRequestId;
        EmployeeId = employeeId;
        AssignedById = assignedById;
        AssignedDate = assignedDate;
        ExpectedReturnDate = expectedReturnDate;
        AssignmentLocation = assignmentLocation;
        ConditionAtAssignment = conditionAtAssignment;
        Remarks = remarks;

        Status = AssetAssignmentStatus.Active;
        IsActive = true;
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

    public bool IsActive { get; private set; }

    public Asset Asset { get; private set; } = null!;

    public AssetRequest AssetRequest { get; private set; } = null!;

    public User Employee { get; private set; } = null!;

    public User AssignedBy { get; private set; } = null!;


    public void Update(
        DateTime? expectedReturnDate,
        string? assignmentLocation,
        string? remarks)
    {
        ExpectedReturnDate = expectedReturnDate;
        AssignmentLocation = assignmentLocation;
        Remarks = remarks;
    }


    public void Return(DateTime actualReturnDate)
    {
        ActualReturnDate = actualReturnDate;
        Status = AssetAssignmentStatus.Returned;
        IsActive = false;
    }


    public void MarkTransferred()
    {
        Status = AssetAssignmentStatus.Transferred;
        IsActive = false;
    }


    public void Cancel()
    {
        Status = AssetAssignmentStatus.Cancelled;
        IsActive = false;
    }


    public void Activate()
    {
        Status = AssetAssignmentStatus.Active;
        IsActive = true;
    }


    public void MarkDeleted(Guid deletedBy)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
    }
}