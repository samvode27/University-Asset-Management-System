namespace UAMS.Application.DTOs.Authentication.Responses;

public class LogoutResponseDto
{
    public bool Succeeded { get; set; }

    public string Message { get; set; } = null!;

    public DateTime LoggedOutAt { get; set; }
}