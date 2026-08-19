namespace UAMS.Application.DTOs.Departments.Responses;

public class DepartmentResponseDto
{
    public Guid Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? OfficeLocation { get; set; }

    public DateTime? EstablishedDate { get; set; }

    public Guid? DepartmentHeadId { get; set; }

    public string? DepartmentHeadName { get; set; }

    public bool IsActive { get; set; }
}