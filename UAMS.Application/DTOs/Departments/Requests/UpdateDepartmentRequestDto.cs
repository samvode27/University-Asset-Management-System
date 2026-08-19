namespace UAMS.Application.DTOs.Departments.Requests;

public class UpdateDepartmentRequestDto
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? OfficeLocation { get; set; }

    public DateTime? EstablishedDate { get; set; }

    public Guid? DepartmentHeadId { get; set; }
}