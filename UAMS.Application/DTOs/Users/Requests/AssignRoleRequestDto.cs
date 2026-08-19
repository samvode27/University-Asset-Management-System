using System.ComponentModel.DataAnnotations;

namespace UAMS.Application.DTOs.Users.Requests;

public class AssignRoleRequestDto
{
    [Required]
    public Guid RoleId { get; set; }
}