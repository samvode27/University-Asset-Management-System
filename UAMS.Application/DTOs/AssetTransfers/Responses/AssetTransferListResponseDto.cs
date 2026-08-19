namespace UAMS.Application.DTOs.AssetTransfers.Responses;

public class AssetTransferListResponseDto
{
    public IReadOnlyCollection<AssetTransferResponseDto> Items { get; set; }
        = Array.Empty<AssetTransferResponseDto>();

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public int TotalPages { get; set; }

    public bool HasPreviousPage { get; set; }

    public bool HasNextPage { get; set; }
}