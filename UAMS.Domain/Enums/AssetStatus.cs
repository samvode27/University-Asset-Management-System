namespace UAMS.Domain.Enums;

public enum AssetStatus
{
    Available = 1,
    Requested = 2,
    PendingDepartmentApproval = 3,
    PendingAssetManagerApproval = 4,
    Assigned = 5,
    InUse = 6,
    TransferPending = 7,
    ReturnPending = 8,
    UnderMaintenance = 9,
    Damaged = 10,
    Unrepairable = 11,
    Disposed = 12
}