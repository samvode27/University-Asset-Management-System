namespace UAMS.Application.DTOs.Roles.Responses;

public class PermissionGroupResponseDto
{
    public string Module { get; set; } = null!;

    public List<PermissionResponseDto> Permissions { get; set; }
        = new();
}