namespace UAMS.Application.DTOs.Authentication.Responses;

public class VerifyEmailResponseDto
{
    public bool Succeeded { get; set; }

    public string Message { get; set; } = null!;

    public DateTime VerifiedAt { get; set; }
}