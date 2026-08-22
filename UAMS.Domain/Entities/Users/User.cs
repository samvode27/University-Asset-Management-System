namespace UAMS.Domain.Entities.Users;

using UAMS.Domain.Common;
using UAMS.Domain.Entities.Departments;
using UAMS.Domain.Entities.Roles;

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

    public bool EmailVerified { get; private set; }

    public DateTime? EmailVerifiedAt { get; private set; }

    public ICollection<UserRole> UserRoles { get; private set; }
        = new List<UserRole>();


    public static User Create(
        string employeeId,
        string fullName,
        string email,
        string phoneNumber,
        Guid departmentId,
        string username,
        string passwordHash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(employeeId);
        ArgumentException.ThrowIfNullOrWhiteSpace(fullName);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(phoneNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);

        return new User
        {
            EmployeeId = employeeId.Trim(),
            FullName = fullName.Trim(),
            Email = email.Trim(),
            PhoneNumber = phoneNumber.Trim(),
            DepartmentId = departmentId,
            Username = username.Trim(),
            PasswordHash = passwordHash,
            IsLocked = false,
            FailedLoginAttempts = 0
        };
    }


    // ================================================================
    // Authentication Behavior
    // ================================================================

    public void ChangePassword(string passwordHash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);

        PasswordHash = passwordHash;

        FailedLoginAttempts = 0;
        IsLocked = false;
        LockedAt = null;
    }


    public void RecordSuccessfulLogin(DateTime loginAt)
    {
        LastLoginAt = loginAt;
        FailedLoginAttempts = 0;
    }


    public void RecordFailedLogin(int maxFailedLoginAttempts)
    {
        if (maxFailedLoginAttempts <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(maxFailedLoginAttempts),
                "Maximum failed login attempts must be greater than zero.");
        }

        if (IsLocked)
        {
            return;
        }

        FailedLoginAttempts++;

        if (FailedLoginAttempts >= maxFailedLoginAttempts)
        {
            LockAccount();
        }
    }


    public void LockAccount()
    {
        IsLocked = true;
        LockedAt = DateTime.UtcNow;
    }


    public void UnlockAccount()
    {
        IsLocked = false;
        FailedLoginAttempts = 0;
        LockedAt = null;
    }

    public void VerifyEmail(DateTime verifiedAt)
    {
        EmailVerified = true;
        EmailVerifiedAt = verifiedAt;
    }


    // ================================================================
    // User Management Behavior
    // ================================================================

    public void UpdateProfile(
        string fullName,
        string email,
        string phoneNumber)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fullName);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(phoneNumber);

        FullName = fullName.Trim();
        Email = email.Trim().ToLowerInvariant();
        PhoneNumber = phoneNumber.Trim();
    }


    public void ChangeDepartment(Guid departmentId)
    {
        if (departmentId == Guid.Empty)
        {
            throw new ArgumentException(
                "Department ID is required.",
                nameof(departmentId));
        }

        DepartmentId = departmentId;
    }


    public void Activate()
    {
        IsActive = true;
    }


    public void Deactivate()
    {
        IsActive = false;
    }


    public void ResetPassword(string passwordHash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);

        PasswordHash = passwordHash;

        FailedLoginAttempts = 0;
        IsLocked = false;
        LockedAt = null;
    }


    public void SoftDelete(Guid deletedBy)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
        IsActive = false;
    }


    public void Restore()
    {
        IsDeleted = false;
        DeletedAt = null;
        DeletedBy = null;
    }
}