namespace UAMS.Application.DTOs.AssetRequests.Requests;

public class AssetManagerReviewRequestDto
{
    /// <summary>
    /// Indicates whether the Asset Manager approves the request.
    /// </summary>
    public bool Approved { get; set; }

    /// <summary>
    /// Optional remarks from the Asset Manager.
    /// </summary>
    public string? Remarks { get; set; }
}