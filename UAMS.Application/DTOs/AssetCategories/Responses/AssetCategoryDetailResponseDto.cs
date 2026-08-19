namespace UAMS.Application.DTOs.AssetCategories.Responses;

public class AssetCategoryDetailResponseDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Code { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int AssetCount { get; set; }
}