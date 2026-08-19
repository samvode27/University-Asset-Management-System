using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.Assets.Requests;

public class AssetFilterRequestDto
{
    /// <summary>
    /// Search by asset tag.
    /// </summary>
    public string? AssetTag { get; set; }

    /// <summary>
    /// Search by asset name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Search by serial number.
    /// </summary>
    public string? SerialNumber { get; set; }

    /// <summary>
    /// Filter by manufacturer.
    /// </summary>
    public string? Manufacturer { get; set; }

    /// <summary>
    /// Filter by model.
    /// </summary>
    public string? Model { get; set; }

    /// <summary>
    /// Filter by category.
    /// </summary>
    public Guid? AssetCategoryId { get; set; }

    /// <summary>
    /// Filter by purchase.
    /// </summary>
    public Guid? PurchaseId { get; set; }

    /// <summary>
    /// Filter by department.
    /// </summary>
    public Guid? DepartmentId { get; set; }

    /// <summary>
    /// Filter by status.
    /// </summary>
    public AssetStatus? Status { get; set; }

    /// <summary>
    /// Filter by physical condition.
    /// </summary>
    public AssetCondition? Condition { get; set; }

    /// <summary>
    /// Filter by current location.
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// Purchase date range - start.
    /// </summary>
    public DateTime? PurchaseDateFrom { get; set; }

    /// <summary>
    /// Purchase date range - end.
    /// </summary>
    public DateTime? PurchaseDateTo { get; set; }

    /// <summary>
    /// Include only active assets.
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Page number.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Number of records per page.
    /// </summary>
    public int PageSize { get; set; } = 20;

    /// <summary>
    /// Sorting property.
    /// </summary>
    public string? SortBy { get; set; }

    /// <summary>
    /// Sort direction.
    /// </summary>
    public bool SortDescending { get; set; }
}