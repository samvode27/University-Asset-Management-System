namespace UAMS.Domain.Enums;

public enum AssetStatus
{
    Available,
    Requested,
    PendingDepartmentApproval,
    PendingAssetManagerApproval,
    Assigned,
    InUse,
    TransferPending,
    ReturnPending,
    UnderMaintenance,
    Damaged,
    Unrepairable,
    Disposed
}