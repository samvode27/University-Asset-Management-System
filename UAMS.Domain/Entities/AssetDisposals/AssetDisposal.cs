using UAMS.Domain.Common;
using UAMS.Domain.Entities.Assets;
using UAMS.Domain.Entities.Maintenances;
using UAMS.Domain.Entities.Users;
using UAMS.Domain.Enums;

namespace UAMS.Domain.Entities.AssetDisposals;

public class AssetDisposal : AuditableEntity
{
    private AssetDisposal()
    {
    }

    public string DisposalNumber { get; private set; } = null!;

    public Guid AssetId { get; private set; }

    public Guid? MaintenanceId { get; private set; }

    public Guid RequestedById { get; private set; }

    public Guid? ApprovedById { get; private set; }

    public Guid? CompletedById { get; private set; }

    public DisposalMethod? DisposalMethod { get; private set; }

    public string Reason { get; private set; } = null!;

    public decimal? BookValue { get; private set; }

    public decimal? EstimatedValue { get; private set; }

    public decimal? DisposalValue { get; private set; }

    public DateTime RequestedDate { get; private set; }

    public DateTime? ApprovedDate { get; private set; }

    public DateTime? DisposalDate { get; private set; }

    public string? Remarks { get; private set; }

    public AssetDisposalStatus Status { get; private set; }


    public Asset Asset { get; private set; } = null!;

    public Maintenance? Maintenance { get; private set; }

    public User RequestedBy { get; private set; } = null!;

    public User? ApprovedBy { get; private set; }

    public User? CompletedBy { get; private set; }


}