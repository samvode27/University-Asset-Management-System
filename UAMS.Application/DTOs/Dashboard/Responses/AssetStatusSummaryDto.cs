namespace UAMS.Application.DTOs.Dashboard.Responses;

public class AssetStatusSummaryDto
{
    public string Status { get; set; } = null!;

    public int Count { get; set; }

    public decimal Percentage { get; set; }
}