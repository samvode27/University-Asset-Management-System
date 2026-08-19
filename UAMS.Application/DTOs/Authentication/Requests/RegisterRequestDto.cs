namespace UAMS.Application.DTOs.Authentication.Requests;

public class RegisterRequestDto
{
    public string EmployeeId { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public Guid DepartmentId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string ConfirmPassword { get; set; } = null!;
}