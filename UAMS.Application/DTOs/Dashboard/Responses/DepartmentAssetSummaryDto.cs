namespace UAMS.Application.DTOs.Dashboard.Responses;

public class DepartmentAssetSummaryDto
{
    public Guid DepartmentId { get; set; }

    public string DepartmentCode { get; set; } = null!;

    public string DepartmentName { get; set; } = null!;

    public int TotalAssets { get; set; }

    public int AssignedAssets { get; set; }

    public int AvailableAssets { get; set; }

    public decimal TotalAssetValue { get; set; }
}