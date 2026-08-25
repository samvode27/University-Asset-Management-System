using UAMS.Domain.Common;
using UAMS.Domain.Entities.Assets;
using UAMS.Domain.Entities.Suppliers;
using UAMS.Domain.Enums;

namespace UAMS.Domain.Entities.Purchases;

public class Purchase : AuditableEntity
{
    private Purchase()
    {
    }


    // ================================================================
    // Properties
    // ================================================================

    public string PurchaseNumber { get; private set; } = null!;

    public Guid SupplierId { get; private set; }

    public DateTime PurchaseDate { get; private set; }

    public string? InvoiceNumber { get; private set; }

    public string? Description { get; private set; }

    public decimal TotalAmount { get; private set; }

    public string Currency { get; private set; } = null!;

    public string? PurchaseOrderNumber { get; private set; }

    public PurchaseStatus Status { get; private set; }


    // ================================================================
    // Navigation Properties
    // ================================================================

    public Supplier Supplier { get; private set; } = null!;

    public ICollection<Asset> Assets { get; private set; }
        = new List<Asset>();


    // ================================================================
    // Factory
    // ================================================================

    public static Purchase Create(
        string purchaseNumber,
        Guid supplierId,
        DateTime purchaseDate,
        string? invoiceNumber,
        string? purchaseOrderNumber,
        string? description,
        decimal totalAmount,
        string currency)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            purchaseNumber);

        ArgumentException.ThrowIfNullOrWhiteSpace(
            currency);

        if (supplierId == Guid.Empty)
        {
            throw new ArgumentException(
                "Supplier ID is required.",
                nameof(supplierId));
        }

        if (totalAmount < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(totalAmount),
                "Total amount cannot be negative.");
        }

        return new Purchase
        {
            PurchaseNumber =
                purchaseNumber.Trim(),

            SupplierId =
                supplierId,

            PurchaseDate =
                purchaseDate,

            InvoiceNumber =
                NormalizeOptional(invoiceNumber),

            PurchaseOrderNumber =
                NormalizeOptional(purchaseOrderNumber),

            Description =
                NormalizeOptional(description),

            TotalAmount =
                totalAmount,

            Currency =
                currency.Trim().ToUpperInvariant(),

            Status =
                PurchaseStatus.Draft,

            IsActive =
                true
        };
    }


    // ================================================================
    // Update
    // ================================================================

    public void Update(
        Guid supplierId,
        DateTime purchaseDate,
        string? invoiceNumber,
        string? purchaseOrderNumber,
        string? description,
        decimal totalAmount,
        string currency)
    {
        if (supplierId == Guid.Empty)
        {
            throw new ArgumentException(
                "Supplier ID is required.",
                nameof(supplierId));
        }

        if (totalAmount < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(totalAmount),
                "Total amount cannot be negative.");
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(
            currency);

        SupplierId =
            supplierId;

        PurchaseDate =
            purchaseDate;

        InvoiceNumber =
            NormalizeOptional(invoiceNumber);

        PurchaseOrderNumber =
            NormalizeOptional(purchaseOrderNumber);

        Description =
            NormalizeOptional(description);

        TotalAmount =
            totalAmount;

        Currency =
            currency.Trim().ToUpperInvariant();
    }


    // ================================================================
    // Status
    // ================================================================

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }


    // ================================================================
    // Private Helpers
    // ================================================================

    private static string? NormalizeOptional(
        string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}