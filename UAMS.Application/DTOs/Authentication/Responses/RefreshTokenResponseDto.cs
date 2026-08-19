namespace UAMS.Application.DTOs.Authentication.Responses;

public class RefreshTokenResponseDto
{
    public bool Succeeded { get; set; }

    public string Message { get; set; } = null!;

    public TokenResponseDto? Tokens { get; set; }
}