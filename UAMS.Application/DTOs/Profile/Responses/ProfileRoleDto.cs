namespace UAMS.Application.DTOs.Profile.Responses;

public class ProfileRoleDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsSystemRole { get; set; }
}