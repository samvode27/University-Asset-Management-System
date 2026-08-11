using UAMS.Domain.Common;
using UAMS.Domain.Entities.Roles;

namespace UAMS.Domain.Entities.Permissions;

public class Permission : AuditableEntity
{
    private Permission()
    {
    }

    public string Name { get; private set; } = null!;

    public string Code { get; private set; } = null!;

    public string? Description { get; private set; }

    public string Module { get; private set; } = null!;


    public ICollection<RolePermission> RolePermissions { get; private set; }
        = new List<RolePermission>();

}