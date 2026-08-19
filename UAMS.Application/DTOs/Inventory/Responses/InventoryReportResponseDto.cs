namespace UAMS.Application.DTOs.Inventory.Responses;

public class InventoryReportResponseDto
{
    public DateTime GeneratedAt { get; set; }

    public string? DepartmentName { get; set; }

    public int TotalAssets { get; set; }

    public decimal TotalValue { get; set; }

    public List<InventoryItemResponseDto> Items { get; set; }
        = new();
}