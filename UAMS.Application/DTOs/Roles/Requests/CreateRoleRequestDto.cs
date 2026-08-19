using System.ComponentModel.DataAnnotations;

namespace UAMS.Application.DTOs.Roles.Requests;

public class CreateRoleRequestDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Code { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    public bool IsSystemRole { get; set; }

    public List<Guid> PermissionIds { get; set; } = new();
}