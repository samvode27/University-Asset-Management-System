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

    public AssetReturn(
        string returnNumber,
        Guid assetId,
        Guid assetAssignmentId,
        Guid returnedById,
        Guid receivedById,
        DateTime returnDate,
        string? returnLocation,
        AssetReturnCondition condition,
        string? remarks)
    {
        ReturnNumber = returnNumber;
        AssetId = assetId;
        AssetAssignmentId = assetAssignmentId;
        ReturnedById = returnedById;
        ReceivedById = receivedById;
        ReturnDate = returnDate;
        ReturnLocation = returnLocation;
        Condition = condition;
        Remarks = remarks;

        Status = AssetReturnStatus.Requested;
        DamageFound = false;
        IsActive = true;
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

    public bool IsActive { get; private set; }

    // Navigation Properties
    public Asset Asset { get; private set; } = null!;

    public AssetAssignment AssetAssignment { get; private set; } = null!;

    public User ReturnedBy { get; private set; } = null!;

    public User ReceivedBy { get; private set; } = null!;

    public User? InspectedBy { get; private set; }

    public DamageReport? DamageReport { get; private set; }

    public void Update(
        DateTime returnDate,
        string? returnLocation,
        AssetReturnCondition condition,
        string? remarks)
    {
        ReturnDate = returnDate;
        ReturnLocation = returnLocation;
        Condition = condition;
        Remarks = remarks;
    }

    public void Approve()
    {
        Status = AssetReturnStatus.Approved;
    }

    public void StartInspection()
    {
        Status = AssetReturnStatus.PendingInspection;
    }

    public void CompleteInspection(
        Guid inspectedById,
        AssetReturnCondition condition,
        string? inspectionNotes)
    {
        InspectedById = inspectedById;
        InspectionDate = DateTime.UtcNow;
        Condition = condition;
        InspectionNotes = inspectionNotes;

        DamageFound =
            condition == AssetReturnCondition.Damaged ||
            condition == AssetReturnCondition.SeverelyDamaged ||
            condition == AssetReturnCondition.MissingParts;

        Status = AssetReturnStatus.Inspected;
    }

    public void LinkDamageReport(Guid damageReportId)
    {
        DamageReportId = damageReportId;
        DamageFound = true;
    }

    public void Complete()
    {
        Status = AssetReturnStatus.Completed;
    }

    public void Reject(string reason)
    {
        Remarks = reason;
        Status = AssetReturnStatus.Rejected;
    }

    public void Cancel()
    {
        Status = AssetReturnStatus.Cancelled;
        IsActive = false;
    }

    public void Activate()
    {
        Status = AssetReturnStatus.Requested;
        IsActive = true;
    }

    public void MarkDeleted(Guid deletedBy)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
    }
}