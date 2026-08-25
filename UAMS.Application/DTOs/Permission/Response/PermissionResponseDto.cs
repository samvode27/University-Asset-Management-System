using UAMS.Domain.Entities.Permissions;

namespace UAMS.Application.DTOs.Permission.Responses;

public class PermissionResponseDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string? Description { get; set; }

    public string Module { get; set; } = null!;

    public bool IsActive { get; set; }
}
