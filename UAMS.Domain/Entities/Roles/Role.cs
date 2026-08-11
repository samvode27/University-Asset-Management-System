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

}