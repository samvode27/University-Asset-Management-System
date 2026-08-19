using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.Purchases.Responses;

public class PurchaseResponseDto
{
    public Guid Id { get; set; }

    public string PurchaseNumber { get; set; } = null!;

    public Guid SupplierId { get; set; }

    public string SupplierName { get; set; } = null!;

    public DateTime PurchaseDate { get; set; }

    public string? InvoiceNumber { get; set; }

    public string? PurchaseOrderNumber { get; set; }

    public string? Description { get; set; }

    public decimal TotalAmount { get; set; }

    public string Currency { get; set; } = null!;

    public PurchaseStatus Status { get; set; }

    public int AssetCount { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}