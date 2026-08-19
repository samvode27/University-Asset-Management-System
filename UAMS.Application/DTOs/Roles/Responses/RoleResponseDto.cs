namespace UAMS.Application.DTOs.Roles.Responses;

public class RoleResponseDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsSystemRole { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}