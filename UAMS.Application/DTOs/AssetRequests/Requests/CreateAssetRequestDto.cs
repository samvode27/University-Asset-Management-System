namespace UAMS.Application.DTOs.AssetRequests.Requests;

public class CreateAssetRequestDto
{
    /// <summary>
    /// Asset being requested.
    /// </summary>
    public Guid AssetId { get; set; }

    /// <summary>
    /// Department making the request.
    /// </summary>
    public Guid DepartmentId { get; set; }

    /// <summary>
    /// Business purpose for requesting the asset.
    /// </summary>
    public string Purpose { get; set; } = null!;

    /// <summary>
    /// Date from which the asset is required.
    /// </summary>
    public DateTime? RequiredFromDate { get; set; }

    /// <summary>
    /// Date until which the asset is required.
    /// </summary>
    public DateTime? RequiredToDate { get; set; }
}