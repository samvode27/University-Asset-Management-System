namespace UAMS.Application.DTOs.Profile.Requests;

public class UpdateProfileRequestDto
{
    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;
}