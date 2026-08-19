namespace UAMS.Application.DTOs.Reports.Responses;

public class AssetReportResponseDto
{
    public Guid AssetId { get; set; }

    public string AssetTag { get; set; } = null!;

    public string AssetName { get; set; } = null!;

    public string CategoryName { get; set; } = null!;

    public string? DepartmentName { get; set; }

    public string Status { get; set; } = null!;

    public string Condition { get; set; } = null!;

    public decimal PurchaseCost { get; set; }

    public DateTime? PurchaseDate { get; set; }
}