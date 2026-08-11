using UAMS.Domain.Common;
using UAMS.Domain.Entities.Departments;
using UAMS.Domain.Entities.Roles;

namespace UAMS.Domain.Entities.Users;

public class User : AuditableEntity
{
    private User()
    {
    }

    public string EmployeeId { get; private set; } = null!;

    public string FullName { get; private set; } = null!;

    public string Email { get; private set; } = null!;

    public string PhoneNumber { get; private set; } = null!;

    public Guid DepartmentId { get; private set; }

    public string Username { get; private set; } = null!;

    public string PasswordHash { get; private set; } = null!;

    public bool IsLocked { get; private set; }

    public int FailedLoginAttempts { get; private set; }

    public DateTime? LastLoginAt { get; private set; }

    public DateTime? LockedAt { get; private set; }

    public Department Department { get; private set; } = null!;

    public ICollection<UserRole> UserRoles { get; private set; }
        = new List<UserRole>();

}