using UAMS.Application.DTOs.Authentication.Responses;
using UAMS.Domain.Entities.Users;

namespace UAMS.Application.Interfaces.Services;

public interface ITokenService
{
    Task<TokenResponseDto> GenerateTokensAsync(
        User user,
        CancellationToken cancellationToken = default);

    Task<TokenResponseDto?> RefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default);

    Task RevokeRefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default);
}