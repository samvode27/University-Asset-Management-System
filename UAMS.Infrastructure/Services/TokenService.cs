using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using UAMS.Application.DTOs.Authentication.Responses;
using UAMS.Application.Interfaces.Persistence;
using UAMS.Application.Interfaces.Services;
using UAMS.Domain.Entities.Users;

namespace UAMS.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;

    public TokenService(
        IConfiguration configuration,
        IUnitOfWork unitOfWork)
    {
        _configuration =
            configuration
            ?? throw new ArgumentNullException(
                nameof(configuration));

        _unitOfWork =
            unitOfWork
            ?? throw new ArgumentNullException(
                nameof(unitOfWork));
    }


    // ================================================================
    // Generate Tokens
    // ================================================================

    public async Task<TokenResponseDto> GenerateTokensAsync(
        User user,
        bool rememberMe,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(user);


        // ============================================================
        // JWT Configuration
        // ============================================================

        var jwtSection =
            _configuration.GetSection("Jwt");

        var key =
            jwtSection["Key"]
            ?? throw new InvalidOperationException(
                "JWT Key is not configured.");

        var issuer =
            jwtSection["Issuer"]
            ?? throw new InvalidOperationException(
                "JWT Issuer is not configured.");

        var audience =
            jwtSection["Audience"]
            ?? throw new InvalidOperationException(
                "JWT Audience is not configured.");


        // ============================================================
        // Token Expiration
        // ============================================================

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
                jwtSection[
                    refreshDaysConfigurationKey],
                out var days)
                ? days
                : defaultRefreshDays;


        var accessTokenExpiresAt =
            DateTime.UtcNow.AddMinutes(
                accessMinutes);


        var refreshTokenExpiresAt =
            DateTime.UtcNow.AddDays(
                refreshDays);


        // ============================================================
        // JWT Claims
        // ============================================================

        var claims =
            new List<Claim>
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


        // ============================================================
        // Create Access Token
        // ============================================================

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


        // ============================================================
        // Create Refresh Token
        // ============================================================

        var refreshToken =
            GenerateRefreshToken();


        var refreshTokenHash =
            HashToken(refreshToken);


        var refreshTokenEntity =
            RefreshToken.Create(
                refreshTokenHash,
                user.Id,
                refreshTokenExpiresAt);


        await _unitOfWork.RefreshTokens.AddAsync(
            refreshTokenEntity,
            cancellationToken);


        await _unitOfWork.SaveChangesAsync(
            cancellationToken);


        // ============================================================
        // Response
        // ============================================================

        return new TokenResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiresAt =
                accessTokenExpiresAt,
            RefreshTokenExpiresAt =
                refreshTokenExpiresAt
        };
    }


    // ================================================================
    // Refresh Token
    // ================================================================

    public async Task<TokenResponseDto?> RefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(
            refreshToken))
        {
            return null;
        }


        // ============================================================
        // Hash Incoming Token
        // ============================================================

        var refreshTokenHash =
            HashToken(refreshToken);


        // ============================================================
        // Find Stored Token
        // ============================================================

        var storedToken =
            await _unitOfWork.RefreshTokens
                .GetByTokenHashAsync(
                    refreshTokenHash,
                    cancellationToken);


        if (storedToken is null)
        {
            return null;
        }


        // ============================================================
        // Validate Token
        // ============================================================

        if (!storedToken.IsActive)
        {
            return null;
        }


        // ============================================================
        // Get User
        // ============================================================

        var user =
            await _unitOfWork.Users
                .GetByIdWithAuthenticationDataAsync(
                    storedToken.UserId,
                    cancellationToken);


        if (user is null ||
            !user.IsActive ||
            user.IsLocked)
        {
            return null;
        }


        // ============================================================
        // Generate New Token Pair
        // ============================================================

        var tokens =
            await GenerateTokensAsync(
                user,
                false,
                cancellationToken);


        // ============================================================
        // Revoke Old Token
        // ============================================================

        var newRefreshTokenHash =
            HashToken(
                tokens.RefreshToken);


        storedToken.Revoke(
            newRefreshTokenHash);


        _unitOfWork.RefreshTokens.Update(
            storedToken);


        await _unitOfWork.SaveChangesAsync(
            cancellationToken);


        return tokens;
    }


    // ================================================================
    // Revoke Refresh Token
    // ================================================================

    public async Task RevokeRefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(
            refreshToken))
        {
            return;
        }


        var refreshTokenHash =
            HashToken(refreshToken);


        var storedToken =
            await _unitOfWork.RefreshTokens
                .GetByTokenHashAsync(
                    refreshTokenHash,
                    cancellationToken);


        if (storedToken is null)
        {
            return;
        }


        if (storedToken.IsRevoked)
        {
            return;
        }


        storedToken.Revoke();


        _unitOfWork.RefreshTokens.Update(
            storedToken);


        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }


    // ================================================================
    // Generate Refresh Token
    // ================================================================

    private static string GenerateRefreshToken()
    {
        var bytes =
            RandomNumberGenerator.GetBytes(64);

        return Convert.ToBase64String(bytes);
    }


    // ================================================================
    // Hash Refresh Token
    // ================================================================

    private static string HashToken(
        string token)
    {
        var bytes =
            SHA256.HashData(
                Encoding.UTF8.GetBytes(token));

        return Convert.ToHexString(bytes);
    }
}