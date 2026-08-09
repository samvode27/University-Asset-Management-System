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

    public AssetDisposal(
        string disposalNumber,
        Guid assetId,
        Guid? maintenanceId,
        Guid requestedById,
        string reason,
        decimal? bookValue,
        decimal? estimatedValue,
        DateTime requestedDate,
        string? remarks)
    {
        DisposalNumber = disposalNumber;
        AssetId = assetId;
        MaintenanceId = maintenanceId;
        RequestedById = requestedById;
        Reason = reason;
        BookValue = bookValue;
        EstimatedValue = estimatedValue;
        RequestedDate = requestedDate;
        Remarks = remarks;

        Status = AssetDisposalStatus.Requested;
        IsActive = true;
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


    public void Update(
        string reason,
        decimal? bookValue,
        decimal? estimatedValue,
        string? remarks)
    {
        Reason = reason;
        BookValue = bookValue;
        EstimatedValue = estimatedValue;
        Remarks = remarks;
    }


    public void StartReview()
    {
        Status = AssetDisposalStatus.UnderReview;
    }


    public void Approve(
        Guid approvedById,
        DisposalMethod disposalMethod)
    {
        ApprovedById = approvedById;
        ApprovedDate = DateTime.UtcNow;
        DisposalMethod = disposalMethod;

        Status = AssetDisposalStatus.Approved;
    }


    public void Reject(string reason)
    {
        Remarks = reason;
        Status = AssetDisposalStatus.Rejected;
    }


    public void Complete(
        Guid completedById,
        decimal? disposalValue,
        string? remarks)
    {
        CompletedById = completedById;
        DisposalValue = disposalValue;
        DisposalDate = DateTime.UtcNow;
        Remarks = remarks;

        Status = AssetDisposalStatus.Completed;
    }


    public void Cancel()
    {
        Status = AssetDisposalStatus.Cancelled;
        IsActive = false;
    }


    public void Activate()
    {
        Status = AssetDisposalStatus.Requested;
        IsActive = true;
    }


    public void MarkDeleted(Guid deletedBy)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
    }
}