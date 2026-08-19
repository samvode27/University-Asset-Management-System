namespace UAMS.Application.DTOs.AssetCategories.Responses;

public class AssetCategoryListResponseDto
{
    public IReadOnlyCollection<AssetCategoryResponseDto> Items { get; set; }
        = Array.Empty<AssetCategoryResponseDto>();

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public int TotalPages { get; set; }
}
