using UAMS.Domain.Common;
using UAMS.Domain.Entities.Users;

namespace UAMS.Domain.Entities.Roles;

public class Role : AuditableEntity
{
    private Role()
    {
    }

    public Role(
        string name,
        string code,
        string? description,
        bool isSystemRole = false)
    {
        Name = name;
        Code = code;
        Description = description;
        IsSystemRole = isSystemRole;
        IsActive = true;
    }

    public string Name { get; private set; } = null!;

    public string Code { get; private set; } = null!;

    public string? Description { get; private set; }

    public bool IsSystemRole { get; private set; }

    public bool IsActive { get; private set; }

    public ICollection<RolePermission> RolePermissions { get; private set; }
        = new List<RolePermission>();

    public ICollection<UserRole> UserRoles { get; private set; }
        = new List<UserRole>();

    public void Update(
        string name,
        string code,
        string? description)
    {
        Name = name;
        Code = code;
        Description = description;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void MarkDeleted(Guid deletedBy)
    {
        if (IsSystemRole)
        {
            throw new InvalidOperationException(
                "System roles cannot be deleted.");
        }

        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
    }
}