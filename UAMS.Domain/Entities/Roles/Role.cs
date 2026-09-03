using UAMS.Domain.Common;
using UAMS.Domain.Entities.Users;

namespace UAMS.Domain.Entities.Roles;

public class Role : AuditableEntity
{
    private Role()
    {
    }


    public string Name { get; private set; } = null!;

    public string Code { get; private set; } = null!;

    public string? Description { get; private set; }

    public bool IsSystemRole { get; private set; }

    public ICollection<RolePermission> RolePermissions { get; private set; }
        = new List<RolePermission>();

    public ICollection<UserRole> UserRoles { get; private set; }
        = new List<UserRole>();


    // ================================================================
    // Factory
    // ================================================================

    public static Role Create(
        string name,
        string code,
        string? description,
        bool isSystemRole)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(code);

        return new Role
        {
            Name = name.Trim(),
            Code = code.Trim(),

            Description =
                string.IsNullOrWhiteSpace(description)
                    ? null
                    : description.Trim(),

            IsSystemRole = isSystemRole,
            IsActive = true
        };
    }


    // ================================================================
    // Update
    // ================================================================

    public void Update(
        string name,
        string code,
        string? description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(code);

        Name = name.Trim();
        Code = code.Trim();

        Description =
            string.IsNullOrWhiteSpace(description)
                ? null
                : description.Trim();
    }


    // ================================================================
    // Activation
    // ================================================================

    public void Activate()
    {
        IsActive = true;
    }


    public void Deactivate()
    {
        IsActive = false;
    }


    // ================================================================
    // Permission Management
    // ================================================================

    public RolePermission? AddPermission(
        Guid permissionId,
        Guid assignedBy)
    {
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

        if (RolePermissions.Any(x =>
            x.PermissionId == permissionId &&
            x.IsActive))
        {
            return null;
        }

        var rolePermission =
            RolePermission.Create(
                Id,
                permissionId,
                assignedBy);

        RolePermissions.Add(rolePermission);

        return rolePermission;
    }


    public void RemovePermission(
        Guid permissionId)
    {
        var rolePermission =
            RolePermissions.FirstOrDefault(x =>
                x.PermissionId == permissionId &&
                x.IsActive);

        if (rolePermission is null)
        {
            return;
        }

        rolePermission.Deactivate();
    }
}