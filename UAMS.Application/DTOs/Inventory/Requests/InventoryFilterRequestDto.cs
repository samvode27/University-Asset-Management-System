using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.Inventory.Requests;

public class InventoryFilterRequestDto
{
    public string? Search { get; set; }

    public Guid? DepartmentId { get; set; }

    public Guid? AssetCategoryId { get; set; }

    public AssetStatus? Status { get; set; }

    public AssetCondition? Condition { get; set; }

    public bool? AssignedOnly { get; set; }

    public bool? AvailableOnly { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}