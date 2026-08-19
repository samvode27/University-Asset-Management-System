namespace UAMS.Application.DTOs.Users.Responses;

public class UserResponseDto
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

    public DateTime? LastLoginAt { get; set; }

    public DateTime CreatedAt { get; set; }
}