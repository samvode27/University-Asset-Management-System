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


    // ================================================================
    // Properties
    // ================================================================

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


    // ================================================================
    // Navigation Properties
    // ================================================================

    public Asset Asset { get; private set; } = null!;

    public AssetAssignment AssetAssignment { get; private set; } = null!;

    public User ReturnedBy { get; private set; } = null!;

    public User ReceivedBy { get; private set; } = null!;

    public User? InspectedBy { get; private set; }

    public DamageReport? DamageReport { get; private set; }


    // ================================================================
    // Factory
    // ================================================================

    public static AssetReturn Create(
        string returnNumber,
        Guid assetId,
        Guid assetAssignmentId,
        Guid returnedById,
        Guid receivedById,
        DateTime returnDate,
        string? returnLocation,
        AssetReturnCondition condition,
        string? inspectionNotes,
        string? remarks)
    {
        if (string.IsNullOrWhiteSpace(returnNumber))
            throw new ArgumentException(
                "Return number is required.",
                nameof(returnNumber));

        if (assetId == Guid.Empty)
            throw new ArgumentException(
                "Asset is required.",
                nameof(assetId));

        if (assetAssignmentId == Guid.Empty)
            throw new ArgumentException(
                "Asset assignment is required.",
                nameof(assetAssignmentId));

        if (returnedById == Guid.Empty)
            throw new ArgumentException(
                "Returned by user is required.",
                nameof(returnedById));

        if (receivedById == Guid.Empty)
            throw new ArgumentException(
                "Received by user is required.",
                nameof(receivedById));

        if (returnDate > DateTime.UtcNow)
            throw new ArgumentException(
                "Return date cannot be in the future.",
                nameof(returnDate));

        return new AssetReturn
        {
            ReturnNumber = returnNumber.Trim(),
            AssetId = assetId,
            AssetAssignmentId = assetAssignmentId,
            ReturnedById = returnedById,
            ReceivedById = receivedById,
            ReturnDate = returnDate,
            ReturnLocation = Normalize(returnLocation),
            Condition = condition,
            InspectionNotes = Normalize(inspectionNotes),
            Remarks = Normalize(remarks),
            DamageFound = false,
            Status = AssetReturnStatus.PendingInspection
        };
    }


    // ================================================================
    // Update
    // ================================================================

    public void Update(
        DateTime returnDate,
        string? returnLocation,
        AssetReturnCondition condition,
        string? inspectionNotes,
        string? remarks)
    {
        EnsureEditable();

        if (returnDate > DateTime.UtcNow)
            throw new ArgumentException(
                "Return date cannot be in the future.",
                nameof(returnDate));

        ReturnDate = returnDate;
        ReturnLocation = Normalize(returnLocation);
        Condition = condition;
        InspectionNotes = Normalize(inspectionNotes);
        Remarks = Normalize(remarks);
    }


    // ================================================================
    // Inspect
    // ================================================================

    public void Inspect(
        Guid inspectedById,
        DateTime inspectionDate,
        bool damageFound,
        string? inspectionNotes,
        Guid? damageReportId,
        string? remarks)
    {
        if (Status != AssetReturnStatus.PendingInspection)
        {
            throw new InvalidOperationException(
                "Only asset returns pending inspection can be inspected.");
        }

        if (inspectedById == Guid.Empty)
        {
            throw new ArgumentException(
                "Inspector is required.",
                nameof(inspectedById));
        }

        if (inspectionDate > DateTime.UtcNow)
        {
            throw new ArgumentException(
                "Inspection date cannot be in the future.",
                nameof(inspectionDate));
        }

        if (damageFound && (!damageReportId.HasValue ||
                            damageReportId.Value == Guid.Empty))
        {
            throw new InvalidOperationException(
                "A damage report is required when damage is found.");
        }

        InspectedById = inspectedById;
        InspectionDate = inspectionDate;
        DamageFound = damageFound;
        DamageReportId = damageReportId;
        InspectionNotes = Normalize(inspectionNotes);
        Remarks = Normalize(remarks);
    }


    // ================================================================
    // Complete
    // ================================================================

    public void Complete(string? remarks)
    {
        if (Status != AssetReturnStatus.PendingInspection)
        {
            throw new InvalidOperationException(
                "Only asset returns pending inspection can be completed.");
        }

        if (!InspectedById.HasValue ||
            !InspectionDate.HasValue)
        {
            throw new InvalidOperationException(
                "The asset return must be inspected before it can be completed.");
        }

        if (DamageFound &&
            (!DamageReportId.HasValue ||
             DamageReportId.Value == Guid.Empty))
        {
            throw new InvalidOperationException(
                "A damage report is required before completing a damaged asset return.");
        }

        Remarks = Normalize(remarks) ?? Remarks;

        Status = AssetReturnStatus.Completed;
    }


    // ================================================================
    // Cancel
    // ================================================================

    public void Cancel(string? reason)
    {
        if (Status == AssetReturnStatus.Completed)
        {
            throw new InvalidOperationException(
                "A completed asset return cannot be cancelled.");
        }

        if (Status == AssetReturnStatus.Cancelled)
        {
            throw new InvalidOperationException(
                "The asset return is already cancelled.");
        }

        Remarks = Normalize(reason) ?? Remarks;

        Status = AssetReturnStatus.Cancelled;
    }


    // ================================================================
    // Helpers
    // ================================================================

    private void EnsureEditable()
    {
        if (Status != AssetReturnStatus.PendingInspection)
        {
            throw new InvalidOperationException(
                "Only asset returns pending inspection can be updated.");
        }
    }


    private static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}