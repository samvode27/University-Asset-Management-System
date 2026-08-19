namespace UAMS.Application.DTOs.Departments.Responses;

public class DepartmentListResponseDto
{
    public IReadOnlyList<DepartmentResponseDto> Items { get; set; }
        = new List<DepartmentResponseDto>();

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public int TotalPages { get; set; }

    public bool HasPreviousPage { get; set; }

    public bool HasNextPage { get; set; }
}