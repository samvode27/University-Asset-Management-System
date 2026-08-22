using UAMS.Domain.Common;
using UAMS.Domain.Entities.Roles;

namespace UAMS.Domain.Entities.Users;

public class UserRole : BaseEntity
{
    private UserRole()
    {
    }

    public Guid UserId { get; private set; }

    public Guid RoleId { get; private set; }

    public DateTime AssignedAt { get; private set; }

    public Guid AssignedBy { get; private set; }

    public bool IsActive { get; private set; }

    public User User { get; private set; } = null!;

    public Role Role { get; private set; } = null!;


    public static UserRole Create(
        Guid userId,
        Guid roleId,
        Guid assignedBy)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException(
                "User ID is required.",
                nameof(userId));
        }

        if (roleId == Guid.Empty)
        {
            throw new ArgumentException(
                "Role ID is required.",
                nameof(roleId));
        }

        if (assignedBy == Guid.Empty)
        {
            throw new ArgumentException(
                "Assigned by user ID is required.",
                nameof(assignedBy));
        }

        return new UserRole
        {
            UserId = userId,
            RoleId = roleId,
            AssignedAt = DateTime.UtcNow,
            AssignedBy = assignedBy,
            IsActive = true
        };
    }


    public void Deactivate()
    {
        IsActive = false;
    }


    public void Activate()
    {
        IsActive = true;
    }
}