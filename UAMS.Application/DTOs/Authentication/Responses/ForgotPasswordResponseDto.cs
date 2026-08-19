namespace UAMS.Application.DTOs.Authentication.Responses;

public class ForgotPasswordResponseDto
{
    public bool Succeeded { get; set; }

    public string Message { get; set; } = null!;

    public DateTime RequestedAt { get; set; }
}