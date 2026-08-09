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

    public AssetRequest(
        string requestNumber,
        Guid assetId,
        Guid requesterId,
        Guid departmentId,
        string purpose,
        DateTime requestedDate,
        DateTime? requiredFromDate,
        DateTime? requiredToDate)
    {
        RequestNumber = requestNumber;
        AssetId = assetId;
        RequesterId = requesterId;
        DepartmentId = departmentId;
        Purpose = purpose;
        RequestedDate = requestedDate;
        RequiredFromDate = requiredFromDate;
        RequiredToDate = requiredToDate;

        Status = AssetRequestStatus.PendingDepartmentHeadApproval;
        IsActive = true;
    }

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

    public bool IsActive { get; private set; }

    public Asset Asset { get; private set; } = null!;

    public User Requester { get; private set; } = null!;

    public Department Department { get; private set; } = null!;

    public User? DepartmentHead { get; private set; }

    public User? AssetManager { get; private set; }


    public void Update(
        string purpose,
        DateTime? requiredFromDate,
        DateTime? requiredToDate)
    {
        Purpose = purpose;
        RequiredFromDate = requiredFromDate;
        RequiredToDate = requiredToDate;
    }


    public void ApproveByDepartmentHead(
        Guid departmentHeadId,
        string? remarks)
    {
        DepartmentHeadId = departmentHeadId;
        DepartmentHeadActionDate = DateTime.UtcNow;
        DepartmentHeadRemarks = remarks;

        Status = AssetRequestStatus.PendingAssetManagerApproval;
    }


    public void RejectByDepartmentHead(
        Guid departmentHeadId,
        string rejectionReason)
    {
        DepartmentHeadId = departmentHeadId;
        DepartmentHeadActionDate = DateTime.UtcNow;
        DepartmentHeadRemarks = rejectionReason;
        RejectionReason = rejectionReason;

        Status = AssetRequestStatus.DepartmentHeadRejected;
    }


    public void ApproveByAssetManager(
        Guid assetManagerId,
        string? remarks)
    {
        AssetManagerId = assetManagerId;
        AssetManagerActionDate = DateTime.UtcNow;
        AssetManagerRemarks = remarks;

        Status = AssetRequestStatus.AssetManagerApproved;
    }


    public void RejectByAssetManager(
        Guid assetManagerId,
        string rejectionReason)
    {
        AssetManagerId = assetManagerId;
        AssetManagerActionDate = DateTime.UtcNow;
        AssetManagerRemarks = rejectionReason;
        RejectionReason = rejectionReason;

        Status = AssetRequestStatus.AssetManagerRejected;
    }


    public void Complete()
    {
        Status = AssetRequestStatus.Completed;
    }


    public void Cancel()
    {
        Status = AssetRequestStatus.Cancelled;
    }


    public void Activate()
    {
        IsActive = true;
    }


    public void Deactivate()
    {
        IsActive = false;
    }


    public void MarkDeleted(Guid deletedBy)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
    }
}