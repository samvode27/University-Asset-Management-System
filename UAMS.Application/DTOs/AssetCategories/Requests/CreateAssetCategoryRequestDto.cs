namespace UAMS.Application.DTOs.AssetCategories.Requests;

public class CreateAssetCategoryRequestDto
{
    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string? Description { get; set; }
}