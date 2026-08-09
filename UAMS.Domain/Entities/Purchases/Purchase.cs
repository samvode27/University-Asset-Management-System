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
    public Purchase(
        string purchaseNumber,
        Guid supplierId,
        DateTime purchaseDate,
        string? invoiceNumber,
        string? purchaseOrderNumber,
        string? description,
        decimal totalAmount,
        string currency)
    {
        PurchaseNumber = purchaseNumber;
        SupplierId = supplierId;
        PurchaseDate = purchaseDate;
        InvoiceNumber = invoiceNumber;
        PurchaseOrderNumber = purchaseOrderNumber;
        Description = description;
        TotalAmount = totalAmount;
        Currency = currency;

        Status = PurchaseStatus.Draft;
        IsActive = true;
    }

    public string PurchaseNumber { get; private set; } = null!;

    public Guid SupplierId { get; private set; }

    public DateTime PurchaseDate { get; private set; }

    public string? InvoiceNumber { get; private set; }

    public string? Description { get; private set; }

    public decimal TotalAmount { get; private set; }

    public string Currency { get; private set; } = null!;

    public Supplier Supplier { get; private set; } = null!;

    public string? PurchaseOrderNumber { get; private set; }

    public PurchaseStatus Status { get; private set; }

    public ICollection<Asset> Assets { get; private set; }
        = new List<Asset>();

    public void Update(
    string purchaseNumber,
    Guid supplierId,
    DateTime purchaseDate,
    string? invoiceNumber,
    string? purchaseOrderNumber,
    string? description,
    decimal totalAmount,
    string currency)
    {
        PurchaseNumber = purchaseNumber;
        SupplierId = supplierId;
        PurchaseDate = purchaseDate;
        InvoiceNumber = invoiceNumber;
        PurchaseOrderNumber = purchaseOrderNumber;
        Description = description;
        TotalAmount = totalAmount;
        Currency = currency;
    }

    public void ChangeSupplier(Guid supplierId)
    {
        SupplierId = supplierId;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void MarkDeleted(Guid deletedBy)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
    }
}