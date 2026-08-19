namespace UAMS.Application.DTOs.Purchases.Responses;

public class PurchaseListResponseDto
{
    public IReadOnlyCollection<PurchaseResponseDto> Items { get; set; }
        = Array.Empty<PurchaseResponseDto>();

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public int TotalPages { get; set; }

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;
}