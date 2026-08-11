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

    public Asset Asset { get; private set; } = null!;

    public User Requester { get; private set; } = null!;

    public Department Department { get; private set; } = null!;

    public User? DepartmentHead { get; private set; }

    public User? AssetManager { get; private set; }


}