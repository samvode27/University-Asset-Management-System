namespace UAMS.Application.DTOs.Departments.Responses;

public class DepartmentDetailResponseDto
{
    public Guid Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? OfficeLocation { get; set; }

    public DateOnly? EstablishedDate { get; set; }

    public Guid? DepartmentHeadId { get; set; }

    public string? DepartmentHeadName { get; set; }

    public bool IsActive { get; set; }

    public int UserCount { get; set; }

    public int AssetCount { get; set; }

    public int AssetRequestCount { get; set; }
}