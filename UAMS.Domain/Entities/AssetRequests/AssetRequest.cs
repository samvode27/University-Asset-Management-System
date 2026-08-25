using UAMS.Domain.Common;
using UAMS.Domain.Entities.Assets;
using UAMS.Domain.Entities.Departments;
using UAMS.Domain.Entities.Users;
using UAMS.Domain.Enums;

namespace UAMS.Domain.Entities.AssetRequests;

public class AssetRequest : AuditableEntity
{
    private AssetRequest()
    {
    }


    // ================================================================
    // Properties
    // ================================================================

    public string RequestNumber { get; private set; } = null!;

    public Guid AssetId { get; private set; }

    public Guid RequesterId { get; private set; }

    public Guid DepartmentId { get; private set; }

    public string Purpose { get; private set; } = null!;

    public DateTime RequestedDate { get; private set; }

    public DateTime? RequiredFromDate { get; private set; }

    public DateTime? RequiredToDate { get; private set; }

    public AssetRequestStatus Status { get; private set; }

    public Guid? DepartmentHeadId { get; private set; }

    public DateTime? DepartmentHeadActionDate { get; private set; }

    public string? DepartmentHeadRemarks { get; private set; }

    public Guid? AssetManagerId { get; private set; }

    public DateTime? AssetManagerActionDate { get; private set; }

    public string? AssetManagerRemarks { get; private set; }

    public string? RejectionReason { get; private set; }


    // ================================================================
    // Navigation Properties
    // ================================================================

    public Asset Asset { get; private set; } = null!;

    public User Requester { get; private set; } = null!;

    public Department Department { get; private set; } = null!;

    public User? DepartmentHead { get; private set; }

    public User? AssetManager { get; private set; }


    // ================================================================
    // Factory
    // ================================================================

    public static AssetRequest Create(
        string requestNumber,
        Guid assetId,
        Guid requesterId,
        Guid departmentId,
        string purpose,
        DateTime requestedDate,
        DateTime? requiredFromDate,
        DateTime? requiredToDate)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(requestNumber);

        if (assetId == Guid.Empty)
        {
            throw new ArgumentException(
                "Asset ID is required.",
                nameof(assetId));
        }

        if (requesterId == Guid.Empty)
        {
            throw new ArgumentException(
                "Requester ID is required.",
                nameof(requesterId));
        }

        if (departmentId == Guid.Empty)
        {
            throw new ArgumentException(
                "Department ID is required.",
                nameof(departmentId));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(purpose);

        ValidateRequiredDates(
            requiredFromDate,
            requiredToDate);

        return new AssetRequest
        {
            Id = Guid.NewGuid(),

            RequestNumber =
                requestNumber.Trim(),

            AssetId =
                assetId,

            RequesterId =
                requesterId,

            DepartmentId =
                departmentId,

            Purpose =
                purpose.Trim(),

            RequestedDate =
                requestedDate,

            RequiredFromDate =
                requiredFromDate,

            RequiredToDate =
                requiredToDate,

            Status =
                AssetRequestStatus.PendingDepartmentHeadApproval,

            IsActive =
                true
        };
    }


    // ================================================================
    // Update
    // ================================================================

    public void Update(
        Guid assetId,
        Guid departmentId,
        string purpose,
        DateTime? requiredFromDate,
        DateTime? requiredToDate)
    {
        if (Status !=
            AssetRequestStatus.PendingDepartmentHeadApproval)
        {
            throw new InvalidOperationException(
                "Only requests pending Department Head approval can be updated.");
        }

        if (assetId == Guid.Empty)
        {
            throw new ArgumentException(
                "Asset ID is required.",
                nameof(assetId));
        }

        if (departmentId == Guid.Empty)
        {
            throw new ArgumentException(
                "Department ID is required.",
                nameof(departmentId));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(purpose);

        ValidateRequiredDates(
            requiredFromDate,
            requiredToDate);

        AssetId =
            assetId;

        DepartmentId =
            departmentId;

        Purpose =
            purpose.Trim();

        RequiredFromDate =
            requiredFromDate;

        RequiredToDate =
            requiredToDate;
    }


    // ================================================================
    // Department Head Review
    // ================================================================

    public void ReviewByDepartmentHead(
        Guid departmentHeadId,
        bool approved,
        DateTime actionDate,
        string? remarks)
    {
        if (departmentHeadId == Guid.Empty)
        {
            throw new ArgumentException(
                "Department Head ID is required.",
                nameof(departmentHeadId));
        }

        if (Status !=
            AssetRequestStatus.PendingDepartmentHeadApproval)
        {
            throw new InvalidOperationException(
                "This request is not pending Department Head approval.");
        }

        DepartmentHeadId =
            departmentHeadId;

        DepartmentHeadActionDate =
            actionDate;

        DepartmentHeadRemarks =
            NormalizeOptional(remarks);

        if (approved)
        {
            Status =
                AssetRequestStatus.DepartmentHeadApproved;

            RejectionReason =
                null;
        }
        else
        {
            Status =
                AssetRequestStatus.DepartmentHeadRejected;

            RejectionReason =
                DepartmentHeadRemarks;
        }
    }


    // ================================================================
    // Move To Asset Manager Approval
    // ================================================================

    public void SubmitToAssetManager()
    {
        if (Status !=
            AssetRequestStatus.DepartmentHeadApproved)
        {
            throw new InvalidOperationException(
                "Only Department Head approved requests can be submitted to the Asset Manager.");
        }

        Status =
            AssetRequestStatus.PendingAssetManagerApproval;
    }


    // ================================================================
    // Asset Manager Review
    // ================================================================

    public void ReviewByAssetManager(
        Guid assetManagerId,
        bool approved,
        DateTime actionDate,
        string? remarks)
    {
        if (assetManagerId == Guid.Empty)
        {
            throw new ArgumentException(
                "Asset Manager ID is required.",
                nameof(assetManagerId));
        }

        if (Status !=
            AssetRequestStatus.PendingAssetManagerApproval)
        {
            throw new InvalidOperationException(
                "This request is not pending Asset Manager approval.");
        }

        AssetManagerId =
            assetManagerId;

        AssetManagerActionDate =
            actionDate;

        AssetManagerRemarks =
            NormalizeOptional(remarks);

        if (approved)
        {
            Status =
                AssetRequestStatus.AssetManagerApproved;

            RejectionReason =
                null;
        }
        else
        {
            Status =
                AssetRequestStatus.AssetManagerRejected;

            RejectionReason =
                AssetManagerRemarks;
        }
    }


    // ================================================================
    // Cancellation
    // ================================================================

    public void Cancel(string? reason)
    {
        var canCancel =
            Status ==
            AssetRequestStatus.PendingDepartmentHeadApproval ||

            Status ==
            AssetRequestStatus.PendingAssetManagerApproval;

        if (!canCancel)
        {
            throw new InvalidOperationException(
                "Only pending asset requests can be cancelled.");
        }

        Status =
            AssetRequestStatus.Cancelled;

        RejectionReason =
            NormalizeOptional(reason);
    }


    // ================================================================
    // Workflow State
    // ================================================================

    public bool RequiresDepartmentHeadAction()
    {
        return Status ==
            AssetRequestStatus.PendingDepartmentHeadApproval;
    }


    public bool RequiresAssetManagerAction()
    {
        return Status ==
            AssetRequestStatus.PendingAssetManagerApproval;
    }


    public bool IsReadyForAssignment()
    {
        return Status ==
            AssetRequestStatus.AssetManagerApproved;
    }


    public bool IsRejected()
    {
        return Status ==
            AssetRequestStatus.DepartmentHeadRejected ||

            Status ==
            AssetRequestStatus.AssetManagerRejected;
    }


    public bool IsCancelled()
    {
        return Status ==
            AssetRequestStatus.Cancelled;
    }


    public bool IsCompleted()
    {
        return Status ==
            AssetRequestStatus.Completed;
    }


    // ================================================================
    // Status
    // ================================================================

    public void Activate()
    {
        IsActive = true;
    }


    public void Deactivate()
    {
        IsActive = false;
    }


    // ================================================================
    // Completion
    // ================================================================

    public void MarkCompleted()
    {
        if (Status !=
            AssetRequestStatus.AssetManagerApproved)
        {
            throw new InvalidOperationException(
                "Only Asset Manager approved requests can be completed.");
        }

        Status =
            AssetRequestStatus.Completed;
    }


    // ================================================================
    // Private Helpers
    // ================================================================

    private static void ValidateRequiredDates(
        DateTime? requiredFromDate,
        DateTime? requiredToDate)
    {
        if (requiredFromDate.HasValue &&
            requiredToDate.HasValue &&
            requiredToDate.Value <
            requiredFromDate.Value)
        {
            throw new ArgumentException(
                "Required to date cannot be earlier than required from date.",
                nameof(requiredToDate));
        }
    }


    private static string? NormalizeOptional(
        string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}