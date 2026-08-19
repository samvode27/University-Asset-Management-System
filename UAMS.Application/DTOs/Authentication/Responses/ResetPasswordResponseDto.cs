namespace UAMS.Application.DTOs.Authentication.Responses;

public class ResetPasswordResponseDto
{
    public bool Succeeded { get; set; }

    public string Message { get; set; } = null!;

    public DateTime ResetAt { get; set; }
}