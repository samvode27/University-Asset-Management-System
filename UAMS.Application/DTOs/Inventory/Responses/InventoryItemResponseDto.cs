namespace UAMS.Application.DTOs.Inventory.Responses;

public class InventoryItemResponseDto
{
    public Guid AssetId { get; set; }

    public string AssetTag { get; set; } = null!;

    public string AssetName { get; set; } = null!;

    public string? SerialNumber { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? DepartmentName { get; set; }

    public string? AssignedEmployeeName { get; set; }

    public string? Location { get; set; }

    public string Condition { get; set; } = null!;

    public string Status { get; set; } = null!;

    public decimal PurchaseCost { get; set; }

    public DateTime? PurchaseDate { get; set; }

    public DateTime? LastInventoryDate { get; set; }
}