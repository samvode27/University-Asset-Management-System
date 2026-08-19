namespace UAMS.Application.DTOs.Suppliers.Requests;

public class SupplierFilterRequestDto
{
    public string? Search { get; set; }

    public bool? IsActive { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}