using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.Purchases.Requests;

public class PurchaseFilterRequestDto
{
    public string? Search { get; set; }

    public Guid? SupplierId { get; set; }

    public DateTime? PurchaseDateFrom { get; set; }

    public DateTime? PurchaseDateTo { get; set; }

    public string? InvoiceNumber { get; set; }

    public string? PurchaseOrderNumber { get; set; }

    public string? Currency { get; set; }

    public decimal? MinimumAmount { get; set; }

    public decimal? MaximumAmount { get; set; }

    public PurchaseStatus? Status { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}