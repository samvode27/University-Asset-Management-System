namespace UAMS.Application.DTOs.Authentication.Responses;

public class CurrentUserResponseDto
{
    public Guid Id { get; set; }

    public string EmployeeId { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Username { get; set; } = null!;

    public Guid DepartmentId { get; set; }

    public string DepartmentName { get; set; } = null!;

    public List<string> Roles { get; set; } = new();

    public List<string> Permissions { get; set; } = new();

    public bool IsActive { get; set; }
}