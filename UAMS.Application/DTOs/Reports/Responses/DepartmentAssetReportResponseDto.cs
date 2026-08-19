namespace UAMS.Application.DTOs.Reports.Responses;

public class DepartmentAssetReportResponseDto
{
    public Guid DepartmentId { get; set; }

    public string DepartmentCode { get; set; } = null!;

    public string DepartmentName { get; set; } = null!;

    public int TotalAssets { get; set; }

    public int AvailableAssets { get; set; }

    public int AssignedAssets { get; set; }

    public int MaintenanceAssets { get; set; }

    public int DamagedAssets { get; set; }

    public decimal TotalAssetValue { get; set; }
}