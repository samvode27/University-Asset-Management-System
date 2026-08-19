using System.ComponentModel.DataAnnotations;

namespace UAMS.Application.DTOs.Roles.Requests;

public class RemovePermissionsRequestDto
{
    [Required]
    [MinLength(1)]
    public List<Guid> PermissionIds { get; set; } = new();
}