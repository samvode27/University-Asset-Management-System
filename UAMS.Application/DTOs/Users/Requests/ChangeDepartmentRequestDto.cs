using System.ComponentModel.DataAnnotations;

namespace UAMS.Application.DTOs.Users.Requests;

public class ChangeDepartmentRequestDto
{
    [Required]
    public Guid DepartmentId { get; set; }

    [StringLength(500)]
    public string? Reason { get; set; }
}