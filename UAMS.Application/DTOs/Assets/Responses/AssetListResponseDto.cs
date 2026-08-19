namespace UAMS.Application.DTOs.Assets.Responses;

public class AssetListResponseDto
{
    public IReadOnlyList<AssetResponseDto> Items { get; set; }
        = Array.Empty<AssetResponseDto>();

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public int TotalPages { get; set; }

    public bool HasPreviousPage { get; set; }

    public bool HasNextPage { get; set; }
}