namespace UAMS.Application.DTOs.AssetReturns.Responses;

public class AssetReturnListResponseDto
{
    public IReadOnlyCollection<AssetReturnResponseDto> Items { get; set; }
        = Array.Empty<AssetReturnResponseDto>();

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public int TotalPages { get; set; }

    public bool HasPreviousPage { get; set; }

    public bool HasNextPage { get; set; }
}