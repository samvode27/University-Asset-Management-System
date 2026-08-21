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

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration
            ?? throw new ArgumentNullException(nameof(configuration));
    }


    public Task<TokenResponseDto> GenerateTokensAsync(
        User user,
        bool rememberMe,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(user);

        var jwtSection = _configuration.GetSection("Jwt");

        var key = jwtSection["Key"]
            ?? throw new InvalidOperationException(
                "JWT Key is not configured.");

        var issuer = jwtSection["Issuer"]
            ?? throw new InvalidOperationException(
                "JWT Issuer is not configured.");

        var audience = jwtSection["Audience"]
            ?? throw new InvalidOperationException(
                "JWT Audience is not configured.");

        var accessMinutes =
            int.TryParse(
                jwtSection["AccessTokenMinutes"],
                out var minutes)
                ? minutes
                : 30;

        var refreshDaysConfigurationKey =
            rememberMe
                ? "RememberMeRefreshTokenDays"
                : "RefreshTokenDays";
        var defaultRefreshDays =
            rememberMe
                ? 30
                : 7;

        var refreshDays =
            int.TryParse(
                jwtSection[refreshDaysConfigurationKey],
                out var days)
                ? days
                : defaultRefreshDays;


        var accessTokenExpiresAt =
            DateTime.UtcNow.AddMinutes(accessMinutes);

        var refreshTokenExpiresAt =
            DateTime.UtcNow.AddDays(refreshDays);


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
                user.Email),

            new(
                "EmployeeId",
                user.EmployeeId),

            new(
                "DepartmentId",
                user.DepartmentId.ToString())
        };


        var securityKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key));

        var credentials =
            new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);


        var jwtToken =
            new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: accessTokenExpiresAt,
                signingCredentials: credentials);


        var accessToken =
            new JwtSecurityTokenHandler()
                .WriteToken(jwtToken);


        var refreshToken =
            GenerateRefreshToken();


        var result = new TokenResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiresAt = accessTokenExpiresAt,
            RefreshTokenExpiresAt = refreshTokenExpiresAt
        };


        return Task.FromResult(result);
    }


    public Task<TokenResponseDto?> RefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        /*
         * IMPORTANT:
         *
         * Persistent refresh-token validation is not implemented
         * here because the current domain model does not yet contain
         * a persisted refresh-token/session entity.
         *
         * This method should be completed after adding persistent
         * refresh-token storage.
         */

        throw new NotSupportedException(
            "Persistent refresh-token storage must be implemented " +
            "before refresh-token rotation is enabled.");
    }


    public Task RevokeRefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        /*
         * Same reason as RefreshTokenAsync().
         */

        throw new NotSupportedException(
            "Persistent refresh-token storage must be implemented " +
            "before refresh-token revocation is enabled.");
    }


    private static string GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);

        return Convert.ToBase64String(bytes);
    }
}