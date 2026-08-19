using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.Assets.Responses;

public class AssetDetailResponseDto
{
    public Guid Id { get; set; }

    public string AssetTag { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? SerialNumber { get; set; }

    public string? Model { get; set; }

    public string? Manufacturer { get; set; }

    // ============================================================
    // Category
    // ============================================================

    public Guid AssetCategoryId { get; set; }

    public string? AssetCategoryName { get; set; }

    // ============================================================
    // Purchase
    // ============================================================

    public Guid PurchaseId { get; set; }

    public string? PurchaseNumber { get; set; }

    public Guid? SupplierId { get; set; }

    public string? SupplierName { get; set; }

    public decimal PurchaseCost { get; set; }

    public DateTime PurchaseDate { get; set; }

    public string? InvoiceNumber { get; set; }

    public string? PurchaseOrderNumber { get; set; }

    // ============================================================
    // Department
    // ============================================================

    public Guid? DepartmentId { get; set; }

    public string? DepartmentName { get; set; }

    // ============================================================
    // Physical Information
    // ============================================================

    public DateTime? WarrantyExpiryDate { get; set; }

    public string? Location { get; set; }

    public AssetStatus Status { get; set; }

    public AssetCondition Condition { get; set; }

    // ============================================================
    // Identification
    // ============================================================

    public bool HasQRCode { get; set; }

    public bool HasBarcode { get; set; }

    // ============================================================
    // Lifecycle Information
    // ============================================================

    public bool HasActiveAssignment { get; set; }

    public bool HasPendingRequest { get; set; }

    public bool HasPendingTransfer { get; set; }

    public bool HasOpenDamageReport { get; set; }

    public bool HasActiveMaintenance { get; set; }

    public bool HasPendingDisposal { get; set; }

    // ============================================================
    // Audit
    // ============================================================

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}