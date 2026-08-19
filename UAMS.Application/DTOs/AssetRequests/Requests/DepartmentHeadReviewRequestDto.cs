namespace UAMS.Application.DTOs.AssetRequests.Requests;

public class DepartmentHeadReviewRequestDto
{
    /// <summary>
    /// Indicates whether the Department Head approves the request.
    /// </summary>
    public bool Approved { get; set; }

    /// <summary>
    /// Optional remarks from the Department Head.
    /// </summary>
    public string? Remarks { get; set; }
}