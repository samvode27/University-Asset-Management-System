namespace UAMS.Application.DTOs.Users.Responses;

public class UserDetailResponseDto
{
    public Guid Id { get; set; }

    public string EmployeeId { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public Guid DepartmentId { get; set; }

    public string DepartmentName { get; set; } = null!;

    public string Username { get; set; } = null!;

    public bool IsActive { get; set; }

    public bool IsLocked { get; set; }

    public int FailedLoginAttempts { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public DateTime? LockedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public List<UserRoleResponseDto> Roles { get; set; } = new();
}