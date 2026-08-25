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


    // ================================================================
    // Properties
    // ================================================================

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


    // ================================================================
    // Navigation Properties
    // ================================================================

    public Asset Asset { get; private set; } = null!;

    public Maintenance? Maintenance { get; private set; }

    public User RequestedBy { get; private set; } = null!;

    public User? ApprovedBy { get; private set; }

    public User? CompletedBy { get; private set; }


    // ================================================================
    // Factory
    // ================================================================

    public static AssetDisposal Create(
        string disposalNumber,
        Guid assetId,
        Guid? maintenanceId,
        Guid requestedById,
        string reason,
        decimal? bookValue,
        decimal? estimatedValue,
        string? remarks)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            disposalNumber,
            nameof(disposalNumber));

        if (assetId == Guid.Empty)
        {
            throw new ArgumentException(
                "Asset is required.",
                nameof(assetId));
        }

        if (requestedById == Guid.Empty)
        {
            throw new ArgumentException(
                "Requested by user is required.",
                nameof(requestedById));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(
            reason,
            nameof(reason));

        ValidateNonNegative(
            bookValue,
            nameof(bookValue),
            "Book value cannot be negative.");

        ValidateNonNegative(
            estimatedValue,
            nameof(estimatedValue),
            "Estimated value cannot be negative.");

        return new AssetDisposal
        {
            DisposalNumber = disposalNumber.Trim(),
            AssetId = assetId,
            MaintenanceId = maintenanceId,
            RequestedById = requestedById,
            Reason = reason.Trim(),
            BookValue = bookValue,
            EstimatedValue = estimatedValue,
            Remarks = Normalize(remarks),
            RequestedDate = DateTime.UtcNow,
            Status = AssetDisposalStatus.Requested
        };
    }


    // ================================================================
    // Update
    // ================================================================

    public void Update(
        Guid? maintenanceId,
        string reason,
        decimal? bookValue,
        decimal? estimatedValue,
        string? remarks)
    {
        if (Status != AssetDisposalStatus.Requested)
        {
            throw new InvalidOperationException(
                "Only requested disposal records can be updated.");
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(
            reason,
            nameof(reason));

        ValidateNonNegative(
            bookValue,
            nameof(bookValue),
            "Book value cannot be negative.");

        ValidateNonNegative(
            estimatedValue,
            nameof(estimatedValue),
            "Estimated value cannot be negative.");

        MaintenanceId = maintenanceId;
        Reason = reason.Trim();
        BookValue = bookValue;
        EstimatedValue = estimatedValue;
        Remarks = Normalize(remarks);
    }


    // ================================================================
    // Start Review
    // ================================================================

    public void StartReview()
    {
        if (Status != AssetDisposalStatus.Requested)
        {
            throw new InvalidOperationException(
                "Only requested disposal records can be placed under review.");
        }

        Status = AssetDisposalStatus.UnderReview;
    }


    // ================================================================
    // Approve
    // ================================================================

    public void Approve(
        Guid approvedById,
        DisposalMethod disposalMethod,
        string? remarks)
    {
        if (approvedById == Guid.Empty)
        {
            throw new ArgumentException(
                "Approving user is required.",
                nameof(approvedById));
        }

        if (Status != AssetDisposalStatus.Requested &&
            Status != AssetDisposalStatus.UnderReview)
        {
            throw new InvalidOperationException(
                "Only requested or under-review disposal records can be approved.");
        }

        DisposalMethod = disposalMethod;
        ApprovedById = approvedById;
        ApprovedDate = DateTime.UtcNow;
        Remarks = Normalize(remarks);

        Status = AssetDisposalStatus.Approved;
    }


    // ================================================================
    // Reject
    // ================================================================

    public void Reject(string reason)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            reason,
            nameof(reason));

        if (Status != AssetDisposalStatus.Requested &&
            Status != AssetDisposalStatus.UnderReview)
        {
            throw new InvalidOperationException(
                "Only requested or under-review disposal records can be rejected.");
        }

        Remarks = reason.Trim();

        Status = AssetDisposalStatus.Rejected;
    }


    // ================================================================
    // Complete
    // ================================================================

    public void Complete(
        Guid completedById,
        DisposalMethod disposalMethod,
        decimal? disposalValue,
        string? remarks)
    {
        if (completedById == Guid.Empty)
        {
            throw new ArgumentException(
                "Completing user is required.",
                nameof(completedById));
        }

        if (Status != AssetDisposalStatus.Approved)
        {
            throw new InvalidOperationException(
                "Only approved disposal records can be completed.");
        }

        ValidateNonNegative(
            disposalValue,
            nameof(disposalValue),
            "Disposal value cannot be negative.");

        DisposalMethod = disposalMethod;
        DisposalValue = disposalValue;
        CompletedById = completedById;
        DisposalDate = DateTime.UtcNow;
        Remarks = Normalize(remarks);

        Status = AssetDisposalStatus.Completed;
    }


    // ================================================================
    // Cancel
    // ================================================================

    public void Cancel(string? remarks)
    {
        if (Status != AssetDisposalStatus.Requested &&
            Status != AssetDisposalStatus.UnderReview)
        {
            throw new InvalidOperationException(
                "Only requested or under-review disposal records can be cancelled.");
        }

        Remarks = Normalize(remarks);

        Status = AssetDisposalStatus.Cancelled;
    }


    // ================================================================
    // Private Helpers
    // ================================================================

    private static void ValidateNonNegative(
        decimal? value,
        string parameterName,
        string message)
    {
        if (value.HasValue && value.Value < 0)
        {
            throw new ArgumentOutOfRangeException(
                parameterName,
                message);
        }
    }


    private static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}