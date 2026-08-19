namespace UAMS.Application.DTOs.Roles.Responses;

public class RolePermissionResponseDto
{
    public Guid Id { get; set; }

    public Guid RoleId { get; set; }

    public Guid PermissionId { get; set; }

    public DateTime AssignedAt { get; set; }

    public Guid AssignedBy { get; set; }

    public bool IsActive { get; set; }

    public PermissionResponseDto Permission { get; set; }
        = null!;
}