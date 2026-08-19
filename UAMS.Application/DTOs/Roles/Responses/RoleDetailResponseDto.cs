namespace UAMS.Application.DTOs.Roles.Responses;

public class RoleDetailResponseDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsSystemRole { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int PermissionCount { get; set; }

    public int UserCount { get; set; }

    public List<PermissionResponseDto> Permissions { get; set; }
        = new();
}