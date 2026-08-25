namespace UAMS.Application.DTOs.Permission.Responses;

public class PermissionListResponseDto
{
    public IReadOnlyList<PermissionResponseDto> Items { get; set; }
        = Array.Empty<PermissionResponseDto>();

    public int TotalCount { get; set; }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int TotalPages { get; set; }
}
