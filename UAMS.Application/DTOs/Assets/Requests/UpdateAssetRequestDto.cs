namespace UAMS.Application.DTOs.Assets.Requests;

public class UpdateAssetRequestDto
{
    /// <summary>
    /// Asset display/name.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Asset description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Serial number.
    /// </summary>
    public string? SerialNumber { get; set; }

    /// <summary>
    /// Asset model.
    /// </summary>
    public string? Model { get; set; }

    /// <summary>
    /// Manufacturer.
    /// </summary>
    public string? Manufacturer { get; set; }

    /// <summary>
    /// Asset category.
    /// </summary>
    public Guid AssetCategoryId { get; set; }

    /// <summary>
    /// Department currently responsible for the asset.
    /// </summary>
    public Guid? DepartmentId { get; set; }

    /// <summary>
    /// Purchase/acquisition cost.
    /// </summary>
    public decimal PurchaseCost { get; set; }

    /// <summary>
    /// Purchase date.
    /// </summary>
    public DateTime PurchaseDate { get; set; }

    /// <summary>
    /// Warranty expiration date.
    /// </summary>
    public DateTime? WarrantyExpiryDate { get; set; }

    /// <summary>
    /// Current location.
    /// </summary>
    public string? Location { get; set; }
}