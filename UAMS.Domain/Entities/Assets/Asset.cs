using UAMS.Domain.Common;
using UAMS.Domain.Entities.AssetAssignments;
using UAMS.Domain.Entities.AssetCategories;
using UAMS.Domain.Entities.AssetDisposals;
using UAMS.Domain.Entities.AssetRequests;
using UAMS.Domain.Entities.AssetReturns;
using UAMS.Domain.Entities.AssetTransfers;
using UAMS.Domain.Entities.Barcodes;
using UAMS.Domain.Entities.DamageReports;
using UAMS.Domain.Entities.Maintenances;
using UAMS.Domain.Entities.Purchases;
using UAMS.Domain.Entities.QRCodes;
using UAMS.Domain.Entities.Departments;
using UAMS.Domain.Enums;

namespace UAMS.Domain.Entities.Assets;

public class Asset : AuditableEntity
{
    private Asset()
    {
    }


    // ================================================================
    // Properties
    // ================================================================

    public string AssetTag { get; private set; } = null!;

    public string Name { get; private set; } = null!;

    public string? Description { get; private set; }

    public string? SerialNumber { get; private set; }

    public string? Model { get; private set; }

    public string? Manufacturer { get; private set; }

    public Guid AssetCategoryId { get; private set; }

    public Guid PurchaseId { get; private set; }

    public Guid? DepartmentId { get; private set; }

    public Department? Department { get; private set; }

    public decimal PurchaseCost { get; private set; }

    public DateTime PurchaseDate { get; private set; }

    public DateTime? WarrantyExpiryDate { get; private set; }

    public string? Location { get; private set; }

    public AssetStatus Status { get; private set; }

    public AssetCondition Condition { get; private set; }

    public AssetCategory AssetCategory { get; private set; } = null!;

    public Purchase Purchase { get; private set; } = null!;

    public QRCode? QRCode { get; private set; }

    public Barcode? Barcode { get; private set; }


    // ================================================================
    // Lifecycle Navigation Collections
    // ================================================================

    public ICollection<AssetRequest> AssetRequests { get; private set; }
        = new List<AssetRequest>();

    public ICollection<AssetAssignment> AssetAssignments { get; private set; }
        = new List<AssetAssignment>();

    public ICollection<AssetTransfer> AssetTransfers { get; private set; }
        = new List<AssetTransfer>();

    public ICollection<AssetReturn> AssetReturns { get; private set; }
        = new List<AssetReturn>();

    public ICollection<DamageReport> DamageReports { get; private set; }
        = new List<DamageReport>();

    public ICollection<Maintenance> Maintenances { get; private set; }
        = new List<Maintenance>();

    public ICollection<AssetDisposal> AssetDisposals { get; private set; }
        = new List<AssetDisposal>();


    // ================================================================
    // Factory
    // ================================================================

    public static Asset Create(
        string assetTag,
        string name,
        string? description,
        string? serialNumber,
        string? model,
        string? manufacturer,
        Guid assetCategoryId,
        Guid purchaseId,
        Guid? departmentId,
        decimal purchaseCost,
        DateTime purchaseDate,
        DateTime? warrantyExpiryDate,
        string? location,
        AssetStatus status,
        AssetCondition condition)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(assetTag);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        if (assetCategoryId == Guid.Empty)
        {
            throw new ArgumentException(
                "Asset category is required.",
                nameof(assetCategoryId));
        }

        if (purchaseId == Guid.Empty)
        {
            throw new ArgumentException(
                "Purchase is required.",
                nameof(purchaseId));
        }

        if (purchaseCost < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(purchaseCost),
                "Purchase cost cannot be negative.");
        }

        if (purchaseDate > DateTime.UtcNow)
        {
            throw new ArgumentException(
                "Purchase date cannot be in the future.",
                nameof(purchaseDate));
        }

        if (warrantyExpiryDate.HasValue &&
            warrantyExpiryDate.Value < purchaseDate)
        {
            throw new ArgumentException(
                "Warranty expiry date cannot be earlier than purchase date.",
                nameof(warrantyExpiryDate));
        }

        return new Asset
        {
            AssetTag = assetTag.Trim(),
            Name = name.Trim(),
            Description = Normalize(description),
            SerialNumber = Normalize(serialNumber),
            Model = Normalize(model),
            Manufacturer = Normalize(manufacturer),
            AssetCategoryId = assetCategoryId,
            PurchaseId = purchaseId,
            DepartmentId = departmentId,
            PurchaseCost = purchaseCost,
            PurchaseDate = purchaseDate,
            WarrantyExpiryDate = warrantyExpiryDate,
            Location = Normalize(location),
            Status = status,
            Condition = condition
        };
    }


    // ================================================================
    // Update
    // ================================================================

    public void Update(
        string name,
        string? description,
        string? serialNumber,
        string? model,
        string? manufacturer,
        Guid assetCategoryId,
        Guid? departmentId,
        decimal purchaseCost,
        DateTime purchaseDate,
        DateTime? warrantyExpiryDate,
        string? location)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        if (assetCategoryId == Guid.Empty)
        {
            throw new ArgumentException(
                "Asset category is required.",
                nameof(assetCategoryId));
        }

        if (purchaseCost < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(purchaseCost),
                "Purchase cost cannot be negative.");
        }

        if (purchaseDate > DateTime.UtcNow)
        {
            throw new ArgumentException(
                "Purchase date cannot be in the future.",
                nameof(purchaseDate));
        }

        if (warrantyExpiryDate.HasValue &&
            warrantyExpiryDate.Value < purchaseDate)
        {
            throw new ArgumentException(
                "Warranty expiry date cannot be earlier than purchase date.",
                nameof(warrantyExpiryDate));
        }

        Name = name.Trim();
        Description = Normalize(description);
        SerialNumber = Normalize(serialNumber);
        Model = Normalize(model);
        Manufacturer = Normalize(manufacturer);
        AssetCategoryId = assetCategoryId;
        DepartmentId = departmentId;
        PurchaseCost = purchaseCost;
        PurchaseDate = purchaseDate;
        WarrantyExpiryDate = warrantyExpiryDate;
        Location = Normalize(location);
    }


    // ================================================================
    // Private Helpers
    // ================================================================

    private static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}