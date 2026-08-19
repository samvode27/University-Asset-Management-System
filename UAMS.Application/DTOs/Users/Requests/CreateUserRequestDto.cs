using System.ComponentModel.DataAnnotations;

namespace UAMS.Application.DTOs.Users.Requests;

public class CreateUserRequestDto
{
    [Required]
    [StringLength(50)]
    public string EmployeeId { get; set; } = null!;

    [Required]
    [StringLength(200)]
    public string FullName { get; set; } = null!;

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = null!;

    [Required]
    [Phone]
    [StringLength(30)]
    public string PhoneNumber { get; set; } = null!;

    [Required]
    public Guid DepartmentId { get; set; }

    [Required]
    [StringLength(100)]
    public string Username { get; set; } = null!;

    [Required]
    [StringLength(100, MinimumLength = 8)]
    public string Password { get; set; } = null!;

    [Required]
    public Guid RoleId { get; set; }
}