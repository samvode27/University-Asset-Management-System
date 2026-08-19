namespace UAMS.Application.DTOs.Purchases.Requests;

public class CreatePurchaseRequestDto
{
    public Guid SupplierId { get; set; }

    public DateTime PurchaseDate { get; set; }

    public string? InvoiceNumber { get; set; }

    public string? PurchaseOrderNumber { get; set; }

    public string? Description { get; set; }

    public decimal TotalAmount { get; set; }

    public string Currency { get; set; } = null!;
}