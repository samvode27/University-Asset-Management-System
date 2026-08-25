using UAMS.Domain.Common;
using UAMS.Domain.Entities.AssetAssignments;
using UAMS.Domain.Entities.Assets;
using UAMS.Domain.Entities.Departments;
using UAMS.Domain.Entities.Users;
using UAMS.Domain.Enums;

namespace UAMS.Domain.Entities.AssetTransfers;

public class AssetTransfer : AuditableEntity
{
    private AssetTransfer()
    {
    }

    public string TransferNumber { get; private set; } = null!;

    public Guid AssetId { get; private set; }

    public Guid AssetAssignmentId { get; private set; }

    public Guid RequestedById { get; private set; }

    public Guid FromEmployeeId { get; private set; }

    public Guid ToEmployeeId { get; private set; }

    public Guid FromDepartmentId { get; private set; }

    public Guid ToDepartmentId { get; private set; }

    public string? FromLocation { get; private set; }

    public string? ToLocation { get; private set; }

    public string Reason { get; private set; } = null!;

    public DateTime RequestedDate { get; private set; }

    public Guid? ApprovedById { get; private set; }

    public DateTime? ApprovedDate { get; private set; }

    public string? ApprovalRemarks { get; private set; }

    public DateTime? CompletedDate { get; private set; }

    public string? Remarks { get; private set; }

    public AssetTransferStatus Status { get; private set; }

    public Asset Asset { get; private set; } = null!;

    public AssetAssignment AssetAssignment { get; private set; } = null!;

    public User RequestedBy { get; private set; } = null!;

    public User FromEmployee { get; private set; } = null!;

    public User ToEmployee { get; private set; } = null!;

    public Department FromDepartment { get; private set; } = null!;

    public Department ToDepartment { get; private set; } = null!;

    public User? ApprovedBy { get; private set; }


    // ================================================================
    // Factory
    // ================================================================

    public static AssetTransfer Create(
        string transferNumber,
        Guid assetId,
        Guid assetAssignmentId,
        Guid requestedById,
        Guid fromEmployeeId,
        Guid toEmployeeId,
        Guid fromDepartmentId,
        Guid toDepartmentId,
        string? fromLocation,
        string? toLocation,
        string reason,
        string? remarks)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(transferNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(reason);

        if (assetId == Guid.Empty)
            throw new ArgumentException(
                "Asset is required.",
                nameof(assetId));

        if (assetAssignmentId == Guid.Empty)
            throw new ArgumentException(
                "Asset assignment is required.",
                nameof(assetAssignmentId));

        if (requestedById == Guid.Empty)
            throw new ArgumentException(
                "Requested by user is required.",
                nameof(requestedById));

        if (fromEmployeeId == Guid.Empty)
            throw new ArgumentException(
                "Source employee is required.",
                nameof(fromEmployeeId));

        if (toEmployeeId == Guid.Empty)
            throw new ArgumentException(
                "Destination employee is required.",
                nameof(toEmployeeId));

        if (fromDepartmentId == Guid.Empty)
            throw new ArgumentException(
                "Source department is required.",
                nameof(fromDepartmentId));

        if (toDepartmentId == Guid.Empty)
            throw new ArgumentException(
                "Destination department is required.",
                nameof(toDepartmentId));

        if (fromEmployeeId == toEmployeeId)
            throw new InvalidOperationException(
                "The source and destination employee cannot be the same.");

        if (fromDepartmentId == toDepartmentId &&
            fromEmployeeId == toEmployeeId)
        {
            throw new InvalidOperationException(
                "The source and destination cannot be identical.");
        }

        return new AssetTransfer
        {
            TransferNumber = transferNumber.Trim(),
            AssetId = assetId,
            AssetAssignmentId = assetAssignmentId,
            RequestedById = requestedById,
            FromEmployeeId = fromEmployeeId,
            ToEmployeeId = toEmployeeId,
            FromDepartmentId = fromDepartmentId,
            ToDepartmentId = toDepartmentId,
            FromLocation = string.IsNullOrWhiteSpace(fromLocation)
                ? null
                : fromLocation.Trim(),
            ToLocation = string.IsNullOrWhiteSpace(toLocation)
                ? null
                : toLocation.Trim(),
            Reason = reason.Trim(),
            RequestedDate = DateTime.UtcNow,
            Remarks = string.IsNullOrWhiteSpace(remarks)
                ? null
                : remarks.Trim(),
            Status = AssetTransferStatus.PendingApproval
        };
    }


    // ================================================================
    // Update
    // ================================================================

    public void Update(
        Guid toEmployeeId,
        Guid toDepartmentId,
        string? toLocation,
        string reason,
        string? remarks)
    {
        EnsurePending();

        if (toEmployeeId == Guid.Empty)
            throw new ArgumentException(
                "Destination employee is required.",
                nameof(toEmployeeId));

        if (toDepartmentId == Guid.Empty)
            throw new ArgumentException(
                "Destination department is required.",
                nameof(toDepartmentId));

        ArgumentException.ThrowIfNullOrWhiteSpace(reason);

        if (toEmployeeId == FromEmployeeId)
            throw new InvalidOperationException(
                "The destination employee cannot be the source employee.");

        ToEmployeeId = toEmployeeId;
        ToDepartmentId = toDepartmentId;

        ToLocation = string.IsNullOrWhiteSpace(toLocation)
            ? null
            : toLocation.Trim();

        Reason = reason.Trim();

        Remarks = string.IsNullOrWhiteSpace(remarks)
            ? null
            : remarks.Trim();
    }


    // ================================================================
    // Approve
    // ================================================================

    public void Approve(
        Guid approvedById,
        string? approvalRemarks)
    {
        if (approvedById == Guid.Empty)
            throw new ArgumentException(
                "Approver is required.",
                nameof(approvedById));

        if (Status != AssetTransferStatus.PendingApproval)
        {
            throw new InvalidOperationException(
                "Only pending asset transfers can be approved.");
        }

        ApprovedById = approvedById;
        ApprovedDate = DateTime.UtcNow;

        ApprovalRemarks =
            string.IsNullOrWhiteSpace(approvalRemarks)
                ? null
                : approvalRemarks.Trim();

        Status = AssetTransferStatus.Approved;
    }


    // ================================================================
    // Reject
    // ================================================================

    public void Reject(
        Guid approvedById,
        string approvalRemarks)
    {
        if (approvedById == Guid.Empty)
            throw new ArgumentException(
                "Approver is required.",
                nameof(approvedById));

        ArgumentException.ThrowIfNullOrWhiteSpace(approvalRemarks);

        if (Status != AssetTransferStatus.PendingApproval)
        {
            throw new InvalidOperationException(
                "Only pending asset transfers can be rejected.");
        }

        ApprovedById = approvedById;
        ApprovedDate = DateTime.UtcNow;
        ApprovalRemarks = approvalRemarks.Trim();

        Status = AssetTransferStatus.Rejected;
    }


    // ================================================================
    // Complete
    // ================================================================

    public void Complete(string? remarks)
    {
        if (Status != AssetTransferStatus.Approved)
        {
            throw new InvalidOperationException(
                "Only approved asset transfers can be completed.");
        }

        CompletedDate = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(remarks))
        {
            Remarks = remarks.Trim();
        }

        Status = AssetTransferStatus.Completed;
    }


    // ================================================================
    // Cancel
    // ================================================================

    public void Cancel(string? reason)
    {
        if (Status == AssetTransferStatus.Completed)
        {
            throw new InvalidOperationException(
                "A completed asset transfer cannot be cancelled.");
        }

        if (Status == AssetTransferStatus.Rejected)
        {
            throw new InvalidOperationException(
                "A rejected asset transfer cannot be cancelled.");
        }

        if (!string.IsNullOrWhiteSpace(reason))
        {
            Remarks = reason.Trim();
        }

        Status = AssetTransferStatus.Cancelled;
    }


    // ================================================================
    // Helpers
    // ================================================================

    private void EnsurePending()
    {
        if (Status != AssetTransferStatus.PendingApproval)
        {
            throw new InvalidOperationException(
                "Only pending asset transfers can be modified.");
        }
    }
}