using UAMS.Domain.Common;
using UAMS.Domain.Entities.Departments;
using UAMS.Domain.Entities.Roles;

namespace UAMS.Domain.Entities.Users;

public class User : AuditableEntity
{
    private User()
    {
    }

    public User(
        string employeeId,
        string fullName,
        string email,
        string phoneNumber,
        Guid departmentId,
        string username,
        string passwordHash)
    {
        EmployeeId = employeeId;
        FullName = fullName;
        Email = email;
        PhoneNumber = phoneNumber;
        DepartmentId = departmentId;
        Username = username;
        PasswordHash = passwordHash;

        IsActive = true;
        IsLocked = false;
        FailedLoginAttempts = 0;
    }

    public string EmployeeId { get; private set; } = null!;

    public string FullName { get; private set; } = null!;

    public string Email { get; private set; } = null!;

    public string PhoneNumber { get; private set; } = null!;

    public Guid DepartmentId { get; private set; }

    public string Username { get; private set; } = null!;

    public string PasswordHash { get; private set; } = null!;

    public bool IsActive { get; private set; }

    public bool IsLocked { get; private set; }

    public int FailedLoginAttempts { get; private set; }

    public DateTime? LastLoginAt { get; private set; }

    public DateTime? LockedAt { get; private set; }

    public Department Department { get; private set; } = null!;

    public ICollection<UserRole> UserRoles { get; private set; }
        = new List<UserRole>();


    public void UpdateProfile(
        string fullName,
        string email,
        string phoneNumber)
    {
        FullName = fullName;
        Email = email;
        PhoneNumber = phoneNumber;
    }


    public void ChangeDepartment(Guid departmentId)
    {
        DepartmentId = departmentId;
    }


    public void ChangePassword(string passwordHash)
    {
        PasswordHash = passwordHash;
    }


    public void Activate()
    {
        IsActive = true;
    }


    public void Deactivate()
    {
        IsActive = false;
    }


    public void RecordFailedLoginAttempt()
    {
        FailedLoginAttempts++;
    }


    public void ResetFailedLoginAttempts()
    {
        FailedLoginAttempts = 0;
    }


    public void Lock()
    {
        IsLocked = true;
        LockedAt = DateTime.UtcNow;
    }


    public void Unlock()
    {
        IsLocked = false;
        LockedAt = null;
        FailedLoginAttempts = 0;
    }


    public void RecordSuccessfulLogin()
    {
        LastLoginAt = DateTime.UtcNow;
        FailedLoginAttempts = 0;
    }
}