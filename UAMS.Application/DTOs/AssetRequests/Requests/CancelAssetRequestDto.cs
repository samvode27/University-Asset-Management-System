namespace UAMS.Application.DTOs.AssetRequests.Requests;

public class CancelAssetRequestDto
{
    /// <summary>
    /// Optional reason for cancelling the request.
    /// </summary>
    public string? Reason { get; set; }
}