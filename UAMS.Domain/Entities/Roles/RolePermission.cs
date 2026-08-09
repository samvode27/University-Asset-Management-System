using UAMS.Domain.Common;
using UAMS.Domain.Entities.Permissions;

namespace UAMS.Domain.Entities.Roles;

public class RolePermission : BaseEntity
{
    private RolePermission()
    {
    }

    public RolePermission(
        Guid roleId,
        Guid permissionId,
        Guid assignedBy)
    {
        RoleId = roleId;
        PermissionId = permissionId;
        AssignedBy = assignedBy;
        AssignedAt = DateTime.UtcNow;
        IsActive = true;
    }

    public Guid RoleId { get; private set; }

    public Guid PermissionId { get; private set; }

    public DateTime AssignedAt { get; private set; }

    public Guid AssignedBy { get; private set; }

    public bool IsActive { get; private set; }

    public Role Role { get; private set; } = null!;

    public Permission Permission { get; private set; } = null!;

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}