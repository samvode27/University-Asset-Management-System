namespace UAMS.Application.DTOs.AssetRequests.Requests;

public class UpdateAssetRequestDto
{
    public Guid AssetId { get; set; }

    public Guid DepartmentId { get; set; }

    public string Purpose { get; set; } = null!;

    public DateTime? RequiredFromDate { get; set; }

    public DateTime? RequiredToDate { get; set; }
}