namespace UAMS.Application.DTOs.Authentication.Responses;

public class ChangePasswordResponseDto
{
    public bool Succeeded { get; set; }

    public string Message { get; set; } = null!;

    public DateTime ChangedAt { get; set; }
}