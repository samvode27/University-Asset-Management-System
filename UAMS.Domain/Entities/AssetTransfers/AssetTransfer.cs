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


}