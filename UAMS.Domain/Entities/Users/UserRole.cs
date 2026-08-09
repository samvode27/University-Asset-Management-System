using UAMS.Domain.Common;
using UAMS.Domain.Entities.Roles;

namespace UAMS.Domain.Entities.Users;

public class UserRole : BaseEntity
{
    private UserRole()
    {
    }

    public UserRole(
        Guid userId,
        Guid roleId,
        Guid assignedBy)
    {
        UserId = userId;
        RoleId = roleId;
        AssignedBy = assignedBy;
        AssignedAt = DateTime.UtcNow;
        IsActive = true;
    }

    public Guid UserId { get; private set; }

    public Guid RoleId { get; private set; }

    public DateTime AssignedAt { get; private set; }

    public Guid AssignedBy { get; private set; }

    public bool IsActive { get; private set; }

    public User User { get; private set; } = null!;

    public Role Role { get; private set; } = null!;

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}