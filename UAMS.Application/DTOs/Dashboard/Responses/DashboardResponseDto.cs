namespace UAMS.Application.DTOs.Dashboard.Responses;

public class DashboardResponseDto
{
    public DashboardSummaryResponseDto Summary { get; set; }
        = new();

    public List<AssetStatusSummaryDto> AssetStatus { get; set; }
        = new();

    public List<AssetCategorySummaryDto> AssetCategories { get; set; }
        = new();

    public List<DepartmentAssetSummaryDto> DepartmentAssets { get; set; }
        = new();

    public List<RecentActivityDto> RecentActivities { get; set; }
        = new();
}