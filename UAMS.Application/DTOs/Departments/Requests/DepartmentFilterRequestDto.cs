namespace UAMS.Application.DTOs.Departments.Requests;

public class DepartmentFilterRequestDto
{
    public string? Search { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public Guid? DepartmentHeadId { get; set; }

    public bool? IsActive { get; set; }

    public DateOnly? EstablishedFrom { get; set; }

    public DateOnly? EstablishedTo { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 20;

    public string? SortBy { get; set; }

    public bool SortDescending { get; set; }
}