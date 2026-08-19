using UAMS.Application.DTOs.Authentication.Requests;
using UAMS.Application.DTOs.Authentication.Responses;

namespace UAMS.Application.Interfaces.Services;

public interface IAuthenticationService
{
    Task<LoginResponseDto> LoginAsync(
        LoginRequestDto request,
        CancellationToken cancellationToken = default);

    Task<LogoutResponseDto> LogoutAsync(
        LogoutRequestDto request,
        CancellationToken cancellationToken = default);

    Task<RefreshTokenResponseDto> RefreshTokenAsync(
        RefreshTokenRequestDto request,
        CancellationToken cancellationToken = default);

    Task<AuthenticationResponseDto> RegisterAsync(
        RegisterRequestDto request,
        CancellationToken cancellationToken = default);

    Task<ChangePasswordResponseDto> ChangePasswordAsync(
        ChangePasswordRequestDto request,
        CancellationToken cancellationToken = default);

    Task<ForgotPasswordResponseDto> ForgotPasswordAsync(
        ForgotPasswordRequestDto request,
        CancellationToken cancellationToken = default);

    Task<ResetPasswordResponseDto> ResetPasswordAsync(
        ResetPasswordRequestDto request,
        CancellationToken cancellationToken = default);

    Task<VerifyEmailResponseDto> VerifyEmailAsync(
        VerifyEmailRequestDto request,
        CancellationToken cancellationToken = default);

    Task<VerifyEmailResponseDto> ResendVerificationEmailAsync(
        ResendVerificationEmailRequestDto request,
        CancellationToken cancellationToken = default);

    Task<TokenResponseDto> RevokeRefreshTokenAsync(
        RevokeRefreshTokenRequestDto request,
        CancellationToken cancellationToken = default);

    Task<CurrentUserResponseDto> GetCurrentUserAsync(
        CancellationToken cancellationToken = default);

    Task<AuthStatusResponseDto> GetAuthStatusAsync(
        CancellationToken cancellationToken = default);
}