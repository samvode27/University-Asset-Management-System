using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.Assets.Requests;

public class CreateAssetRequestDto
{
    /// <summary>
    /// Unique university asset tag.
    /// Example: AST-2026-000001
    /// </summary>
    public string AssetTag { get; set; } = null!;

    /// <summary>
    /// Asset name.
    /// Example: Dell Latitude 5550
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Optional detailed description of the asset.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Manufacturer serial number.
    /// </summary>
    public string? SerialNumber { get; set; }

    /// <summary>
    /// Asset model.
    /// </summary>
    public string? Model { get; set; }

    /// <summary>
    /// Asset manufacturer.
    /// </summary>
    public string? Manufacturer { get; set; }

    /// <summary>
    /// Asset category.
    /// </summary>
    public Guid AssetCategoryId { get; set; }

    /// <summary>
    /// Purchase record associated with this asset.
    /// </summary>
    public Guid PurchaseId { get; set; }

    /// <summary>
    /// Optional department currently responsible for the asset.
    /// </summary>
    public Guid? DepartmentId { get; set; }

    /// <summary>
    /// Purchase/acquisition cost of the asset.
    /// </summary>
    public decimal PurchaseCost { get; set; }

    /// <summary>
    /// Date on which the asset was purchased.
    /// </summary>
    public DateTime PurchaseDate { get; set; }

    /// <summary>
    /// Optional warranty expiration date.
    /// </summary>
    public DateTime? WarrantyExpiryDate { get; set; }

    /// <summary>
    /// Current physical/system location.
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// Initial asset status.
    /// Usually determined by the application workflow.
    /// </summary>
    public AssetStatus Status { get; set; }

    /// <summary>
    /// Initial physical condition of the asset.
    /// </summary>
    public AssetCondition Condition { get; set; }
}