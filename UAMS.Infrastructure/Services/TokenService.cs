using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UAMS.Application.DTOs.Authentication.Responses;
using UAMS.Application.Interfaces.Services;
using UAMS.Domain.Entities.Users;

namespace UAMS.Infrastructure.Services;

public sealed class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<TokenResponseDto> GenerateTokensAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(user);

        var jwtKey = _configuration["Jwt:Key"];

        if (string.IsNullOrWhiteSpace(jwtKey))
        {
            throw new InvalidOperationException(
                "JWT signing key is not configured.");
        }

        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];

        var accessTokenMinutes =
            _configuration.GetValue<int?>(
                "Jwt:AccessTokenExpirationMinutes")
            ?? 30;

        var refreshTokenDays =
            _configuration.GetValue<int?>(
                "Jwt:RefreshTokenExpirationDays")
            ?? 7;

        var now = DateTime.UtcNow;
        var expiresAt = now.AddMinutes(accessTokenMinutes);

        var claims = new List<Claim>
        {
            new(
                ClaimTypes.NameIdentifier,
                user.Id.ToString()),

            new(
                ClaimTypes.Name,
                user.Username),

            new(
                ClaimTypes.Email,
                user.Email)
        };

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: now,
            expires: expiresAt,
            signingCredentials: credentials);

        var accessToken =
            new JwtSecurityTokenHandler()
                .WriteToken(token);

        var refreshToken =
            GenerateRefreshToken();

        var response = new TokenResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = expiresAt
        };

        return Task.FromResult(response);
    }

    public Task<TokenResponseDto?> RefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        /*
         * Refresh-token persistence should be implemented together
         * with the refresh-token repository/entity.
         *
         * Do not simply accept an arbitrary refresh token.
         */

        return Task.FromResult<TokenResponseDto?>(null);
    }

    public Task RevokeRefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(refreshToken);

        /*
         * Revoke the persisted refresh token here.
         */

        return Task.CompletedTask;
    }

    private static string GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);

        return Convert.ToBase64String(bytes);
    }
}