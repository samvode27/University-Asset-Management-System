namespace UAMS.Application.DTOs.Barcode.Responses;

public class BarcodeListResponseDto
{
    public IReadOnlyList<BarcodeResponseDto> Items { get; set; }
        = Array.Empty<BarcodeResponseDto>();

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public int TotalPages { get; set; }

    public bool HasPreviousPage { get; set; }

    public bool HasNextPage { get; set; }
}