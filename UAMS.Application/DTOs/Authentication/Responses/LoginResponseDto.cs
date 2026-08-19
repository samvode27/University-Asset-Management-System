namespace UAMS.Application.DTOs.Authentication.Responses;

public class LoginResponseDto
{
    public bool Succeeded { get; set; }

    public string Message { get; set; } = null!;

    public TokenResponseDto? Tokens { get; set; }

    public CurrentUserResponseDto? User { get; set; }

    public UserSessionResponseDto? Session { get; set; }
}