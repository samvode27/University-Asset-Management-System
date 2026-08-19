namespace UAMS.Application.DTOs.Dashboard.Responses;

public class AssetCategorySummaryDto
{
    public Guid CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public int AssetCount { get; set; }

    public decimal TotalValue { get; set; }
}