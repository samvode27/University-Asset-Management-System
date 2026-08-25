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

    // ================================================================
    // Properties
    // ================================================================

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


    // ================================================================
    // Navigation Properties
    // ================================================================

    public Asset Asset { get; private set; } = null!;

    public AssetRequest AssetRequest { get; private set; } = null!;

    public User Employee { get; private set; } = null!;

    public User AssignedBy { get; private set; } = null!;


    // ================================================================
    // Factory
    // ================================================================

    public static AssetAssignment Create(
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
        ArgumentException.ThrowIfNullOrWhiteSpace(
            assignmentNumber);

        if (assetId == Guid.Empty)
        {
            throw new ArgumentException(
                "Asset ID is required.",
                nameof(assetId));
        }

        if (assetRequestId == Guid.Empty)
        {
            throw new ArgumentException(
                "Asset request ID is required.",
                nameof(assetRequestId));
        }

        if (employeeId == Guid.Empty)
        {
            throw new ArgumentException(
                "Employee ID is required.",
                nameof(employeeId));
        }

        if (assignedById == Guid.Empty)
        {
            throw new ArgumentException(
                "Assigned by user ID is required.",
                nameof(assignedById));
        }

        if (expectedReturnDate.HasValue &&
            expectedReturnDate.Value.Date < assignedDate.Date)
        {
            throw new ArgumentException(
                "Expected return date cannot be earlier than assigned date.",
                nameof(expectedReturnDate));
        }

        return new AssetAssignment
        {
            Id = Guid.NewGuid(),

            AssignmentNumber =
                assignmentNumber.Trim(),

            AssetId =
                assetId,

            AssetRequestId =
                assetRequestId,

            EmployeeId =
                employeeId,

            AssignedById =
                assignedById,

            AssignedDate =
                assignedDate,

            ExpectedReturnDate =
                expectedReturnDate,

            AssignmentLocation =
                NormalizeOptional(assignmentLocation),

            ConditionAtAssignment =
                conditionAtAssignment,

            Remarks =
                NormalizeOptional(remarks),

            Status =
                AssetAssignmentStatus.Active,

            IsActive =
                true
        };
    }


    // ================================================================
    // Update
    // ================================================================

    public void Update(
        DateTime? expectedReturnDate,
        string? assignmentLocation,
        string? remarks)
    {
        if (Status != AssetAssignmentStatus.Active)
        {
            throw new InvalidOperationException(
                "Only active asset assignments can be updated.");
        }

        if (expectedReturnDate.HasValue &&
            expectedReturnDate.Value.Date < AssignedDate.Date)
        {
            throw new ArgumentException(
                "Expected return date cannot be earlier than assigned date.",
                nameof(expectedReturnDate));
        }

        ExpectedReturnDate =
            expectedReturnDate;

        AssignmentLocation =
            NormalizeOptional(assignmentLocation);

        Remarks =
            NormalizeOptional(remarks);
    }


    // ================================================================
    // Complete / Return
    // ================================================================

    public void Complete(
        DateTime actualReturnDate)
    {
        if (Status != AssetAssignmentStatus.Active)
        {
            throw new InvalidOperationException(
                "Only active asset assignments can be completed.");
        }

        if (actualReturnDate < AssignedDate)
        {
            throw new ArgumentException(
                "Actual return date cannot be earlier than assigned date.",
                nameof(actualReturnDate));
        }

        ActualReturnDate =
            actualReturnDate;

        Status =
            AssetAssignmentStatus.Returned;

        IsActive =
            false;
    }


    // ================================================================
    // Transfer
    // ================================================================

    public void MarkAsTransferred()
    {
        if (Status != AssetAssignmentStatus.Active)
        {
            throw new InvalidOperationException(
                "Only active asset assignments can be transferred.");
        }

        Status =
            AssetAssignmentStatus.Transferred;

        IsActive =
            false;
    }


    // ================================================================
    // Cancel
    // ================================================================

    public void Cancel(string? reason)
    {
        if (Status != AssetAssignmentStatus.Active)
        {
            throw new InvalidOperationException(
                "Only active asset assignments can be cancelled.");
        }

        Status =
            AssetAssignmentStatus.Cancelled;

        Remarks =
            NormalizeOptional(reason);

        IsActive =
            false;
    }


    // ================================================================
    // Workflow State
    // ================================================================

    public bool IsActiveAssignment()
    {
        return Status == AssetAssignmentStatus.Active &&
               ActualReturnDate == null &&
               IsActive;
    }


    public bool IsReturned()
    {
        return Status == AssetAssignmentStatus.Returned &&
               ActualReturnDate.HasValue;
    }


    public bool IsTransferred()
    {
        return Status == AssetAssignmentStatus.Transferred;
    }


    public bool IsCancelled()
    {
        return Status == AssetAssignmentStatus.Cancelled;
    }


    // ================================================================
    // Private Helpers
    // ================================================================

    private static string? NormalizeOptional(
        string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}