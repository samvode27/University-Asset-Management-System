using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.Assets.Responses;

public class AssetResponseDto
{
    public Guid Id { get; set; }

    public string AssetTag { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? SerialNumber { get; set; }

    public string? Model { get; set; }

    public string? Manufacturer { get; set; }

    public Guid AssetCategoryId { get; set; }

    public string? AssetCategoryName { get; set; }

    public Guid PurchaseId { get; set; }

    public string? PurchaseNumber { get; set; }

    public Guid? DepartmentId { get; set; }

    public string? DepartmentName { get; set; }

    public decimal PurchaseCost { get; set; }

    public DateTime PurchaseDate { get; set; }

    public DateTime? WarrantyExpiryDate { get; set; }

    public string? Location { get; set; }

    public AssetStatus Status { get; set; }

    public AssetCondition Condition { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}