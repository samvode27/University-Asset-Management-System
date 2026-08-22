using UAMS.Domain.Common;
using UAMS.Domain.Entities.Permissions;

namespace UAMS.Domain.Entities.Roles;

public class RolePermission : BaseEntity
{
    private RolePermission()
    {
    }


    public Guid RoleId { get; private set; }

    public Guid PermissionId { get; private set; }

    public DateTime AssignedAt { get; private set; }

    public Guid AssignedBy { get; private set; }

    public bool IsActive { get; private set; }

    public Role Role { get; private set; } = null!;

    public Permission Permission { get; private set; } = null!;


    // ================================================================
    // Factory
    // ================================================================

    public static RolePermission Create(
        Guid roleId,
        Guid permissionId,
        Guid assignedBy)
    {
        if (roleId == Guid.Empty)
        {
            throw new ArgumentException(
                "Role ID is required.",
                nameof(roleId));
        }

        if (permissionId == Guid.Empty)
        {
            throw new ArgumentException(
                "Permission ID is required.",
                nameof(permissionId));
        }

        if (assignedBy == Guid.Empty)
        {
            throw new ArgumentException(
                "Assigned by user ID is required.",
                nameof(assignedBy));
        }

        return new RolePermission
        {
            RoleId = roleId,
            PermissionId = permissionId,
            AssignedAt = DateTime.UtcNow,
            AssignedBy = assignedBy,
            IsActive = true
        };
    }


    // ================================================================
    // Activation
    // ================================================================

    public void Deactivate()
    {
        IsActive = false;
    }
}