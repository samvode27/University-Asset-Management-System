namespace UAMS.Application.DTOs.Purchases.Responses;

public class PurchaseAssetSummaryDto
{
    public Guid Id { get; set; }

    public string AssetTag { get; set; } = null!;

    public string AssetName { get; set; } = null!;

    public Guid AssetCategoryId { get; set; }

    public string? SerialNumber { get; set; }

    public decimal PurchaseCost { get; set; }

    public bool IsActive { get; set; }
}