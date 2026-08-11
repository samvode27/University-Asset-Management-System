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

}