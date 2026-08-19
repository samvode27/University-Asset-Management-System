namespace UAMS.Application.DTOs.Authentication.Responses;

public class AuthenticationResponseDto
{
    public bool Succeeded { get; set; }

    public string Message { get; set; } = null!;

    public TokenResponseDto? Tokens { get; set; }

    public CurrentUserResponseDto? User { get; set; }
}