namespace UAMS.Domain.Enums;

public enum AssetRequestStatus
{
    PendingDepartmentHeadApproval,
    DepartmentHeadApproved,
    DepartmentHeadRejected,
    PendingAssetManagerApproval,
    AssetManagerApproved,
    AssetManagerRejected,
    Cancelled,
    Completed
}