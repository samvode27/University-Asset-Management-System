namespace UAMS.Application.DTOs.DamageReports.Responses;

public class DamageReportListResponseDto
{
    public IReadOnlyList<DamageReportResponseDto> Items { get; set; }
        = new List<DamageReportResponseDto>();

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public int TotalPages { get; set; }

    public bool HasPreviousPage { get; set; }

    public bool HasNextPage { get; set; }
}