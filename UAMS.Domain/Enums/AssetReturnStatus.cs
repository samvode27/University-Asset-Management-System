namespace UAMS.Domain.Enums;

public enum AssetReturnStatus
{
    Requested = 1,
    Approved = 2,
    PendingInspection = 3,
    Inspected = 4,
    Completed = 5,
    Rejected = 6,
    Cancelled = 7
}