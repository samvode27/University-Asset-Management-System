using System.ComponentModel.DataAnnotations;

namespace UAMS.Application.DTOs.Users.Requests;

public class ResetUserPasswordRequestDto
{
    [Required]
    [StringLength(100, MinimumLength = 8)]
    public string NewPassword { get; set; } = null!;

    [Required]
    [Compare(nameof(NewPassword))]
    public string ConfirmPassword { get; set; } = null!;
}