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
    
    public ICollection<Maintenance> Maintenances
        { get; private set; }
        = new List<Maintenance>();

    public ICollection<AssetDisposal> AssetDisposals
        { get; private set; }
        = new List<AssetDisposal>();


}