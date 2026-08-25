namespace UAMS.Application.DTOs.Permission.Requests;

public class CreatePermissionRequestDto
{
    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string? Description { get; set; }

    public string Module { get; set; } = null!;

    public Guid? CreatedBy { get; set; }
}

