namespace UAMS.Application.DTOs.Authentication.Requests;

public class RevokeRefreshTokenRequestDto
{
    public string RefreshToken { get; set; } = null!;
}