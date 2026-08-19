namespace UAMS.Application.DTOs.Suppliers.Responses;

public class SupplierListResponseDto
{
    public IReadOnlyCollection<SupplierResponseDto> Items { get; set; }
        = Array.Empty<SupplierResponseDto>();

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public int TotalPages { get; set; }

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;
}