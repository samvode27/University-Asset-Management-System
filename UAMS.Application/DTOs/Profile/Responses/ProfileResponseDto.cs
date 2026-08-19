namespace UAMS.Application.DTOs.Profile.Responses;

public class ProfileResponseDto
{
    public Guid Id { get; set; }

    public string EmployeeId { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Username { get; set; } = null!;


    // ============================================================
    // Department
    // ============================================================

    public Guid DepartmentId { get; set; }

    public string DepartmentCode { get; set; } = null!;

    public string DepartmentName { get; set; } = null!;


    // ============================================================
    // Roles
    // ============================================================

    public List<ProfileRoleDto> Roles { get; set; }
        = new();


    // ============================================================
    // Account Information
    // ============================================================

    public bool IsActive { get; set; }

    public DateTime? LastLoginAt { get; set; }
}