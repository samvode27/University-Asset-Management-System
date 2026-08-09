namespace UAMS.Domain.Enums;

public enum AssetRequestStatus
{
    PendingDepartmentHeadApproval = 1,
    DepartmentHeadApproved = 2,
    DepartmentHeadRejected = 3,
    PendingAssetManagerApproval = 4,
    AssetManagerApproved = 5,
    AssetManagerRejected = 6,
    Cancelled = 7,
    Completed = 8
}