namespace UAMS.Application.DTOs.Inventory.Responses;

public class InventorySummaryResponseDto
{
    public int TotalAssets { get; set; }

    public int AvailableAssets { get; set; }

    public int AssignedAssets { get; set; }

    public int MaintenanceAssets { get; set; }

    public int DamagedAssets { get; set; }

    public int DisposedAssets { get; set; }

    public decimal TotalPurchaseValue { get; set; }

    public decimal TotalCurrentValue { get; set; }
}