using System.ComponentModel.DataAnnotations;

namespace UAMS.Application.DTOs.Users.Requests;

public class UpdateUserRequestDto
{
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

    public bool IsActive { get; set; }
}