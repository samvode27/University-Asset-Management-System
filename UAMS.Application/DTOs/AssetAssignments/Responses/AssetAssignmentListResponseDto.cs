namespace UAMS.Application.DTOs.AssetAssignments.Responses;

public class AssetAssignmentListResponseDto
{
    public IReadOnlyCollection<AssetAssignmentResponseDto> Items { get; set; }
        = Array.Empty<AssetAssignmentResponseDto>();

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public int TotalPages { get; set; }

    public bool HasPreviousPage { get; set; }

    public bool HasNextPage { get; set; }
}