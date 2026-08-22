using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using UAMS.Application.DTOs.Authentication.Requests;
using UAMS.Application.DTOs.Authentication.Responses;
using UAMS.Application.Interfaces.Services;

namespace UAMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(
        IAuthenticationService authenticationService)
    {
        _authenticationService =
            authenticationService
            ?? throw new ArgumentNullException(
                nameof(authenticationService));
    }


    // ================================================================
    // Login
    // ================================================================

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(
        typeof(LoginResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        typeof(LoginResponseDto),
        StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LoginResponseDto>> Login(
        [FromBody] LoginRequestDto request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response =
            await _authenticationService.LoginAsync(
                request,
                cancellationToken);

        if (!response.Succeeded)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }


    // ================================================================
    // Register
    // ================================================================

    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(
        typeof(AuthenticationResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        typeof(AuthenticationResponseDto),
        StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthenticationResponseDto>> Register(
        [FromBody] RegisterRequestDto request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response =
            await _authenticationService.RegisterAsync(
                request,
                cancellationToken);

        if (!response.Succeeded)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }


    // ================================================================
    // Logout
    // ================================================================

    [HttpPost("logout")]
    [AllowAnonymous]
    [ProducesResponseType(
        typeof(LogoutResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        typeof(LogoutResponseDto),
        StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LogoutResponseDto>> Logout(
        [FromBody] LogoutRequestDto request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response =
            await _authenticationService.LogoutAsync(
                request,
                cancellationToken);

        if (!response.Succeeded)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }


    // ================================================================
    // Refresh Token
    // ================================================================

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    [ProducesResponseType(
        typeof(RefreshTokenResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        typeof(RefreshTokenResponseDto),
        StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RefreshTokenResponseDto>> RefreshToken(
        [FromBody] RefreshTokenRequestDto request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response =
            await _authenticationService.RefreshTokenAsync(
                request,
                cancellationToken);

        if (!response.Succeeded)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }


    // ================================================================
    // Change Password
    // ================================================================

    [HttpPost("change-password")]
    [Authorize]
    [ProducesResponseType(
        typeof(ChangePasswordResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        typeof(ChangePasswordResponseDto),
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ChangePasswordResponseDto>>
        ChangePassword(
            [FromBody] ChangePasswordRequestDto request,
            CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response =
            await _authenticationService.ChangePasswordAsync(
                request,
                cancellationToken);

        if (!response.Succeeded)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }


    // ================================================================
    // Forgot Password
    // ================================================================

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    [ProducesResponseType(
        typeof(ForgotPasswordResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        typeof(ForgotPasswordResponseDto),
        StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ForgotPasswordResponseDto>>
        ForgotPassword(
            [FromBody] ForgotPasswordRequestDto request,
            CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response =
            await _authenticationService.ForgotPasswordAsync(
                request,
                cancellationToken);

        if (!response.Succeeded)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }


    // ================================================================
    // Reset Password
    // ================================================================

    [HttpPost("reset-password")]
    [AllowAnonymous]
    [ProducesResponseType(
        typeof(ResetPasswordResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        typeof(ResetPasswordResponseDto),
        StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ResetPasswordResponseDto>>
        ResetPassword(
            [FromBody] ResetPasswordRequestDto request,
            CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response =
            await _authenticationService.ResetPasswordAsync(
                request,
                cancellationToken);

        if (!response.Succeeded)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }


    // ================================================================
    // Verify Email
    // ================================================================

    [HttpPost("verify-email")]
    [AllowAnonymous]
    [ProducesResponseType(
        typeof(VerifyEmailResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        typeof(VerifyEmailResponseDto),
        StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<VerifyEmailResponseDto>>
        VerifyEmail(
            [FromBody] VerifyEmailRequestDto request,
            CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response =
            await _authenticationService.VerifyEmailAsync(
                request,
                cancellationToken);

        if (!response.Succeeded)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }


    // ================================================================
    // Resend Verification Email
    // ================================================================

    [HttpPost("resend-verification-email")]
    [AllowAnonymous]
    [ProducesResponseType(
        typeof(VerifyEmailResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        typeof(VerifyEmailResponseDto),
        StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<VerifyEmailResponseDto>>
        ResendVerificationEmail(
            [FromBody] ResendVerificationEmailRequestDto request,
            CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response =
            await _authenticationService
                .ResendVerificationEmailAsync(
                    request,
                    cancellationToken);

        if (!response.Succeeded)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }


    // ================================================================
    // Revoke Refresh Token
    // ================================================================

    [HttpPost("revoke-refresh-token")]
    [AllowAnonymous]
    [ProducesResponseType(
        typeof(TokenResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TokenResponseDto>>
        RevokeRefreshToken(
            [FromBody] RevokeRefreshTokenRequestDto request,
            CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            return BadRequest(
                new
                {
                    Message = "Refresh token is required."
                });
        }

        var response =
            await _authenticationService
                .RevokeRefreshTokenAsync(
                    request,
                    cancellationToken);

        return Ok(response);
    }


    // ================================================================
    // Current User
    // ================================================================

    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(
        typeof(CurrentUserResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<CurrentUserResponseDto>>
        GetCurrentUser(
            CancellationToken cancellationToken)
    {
        var response =
            await _authenticationService
                .GetCurrentUserAsync(
                    cancellationToken);

        return Ok(response);
    }


    // ================================================================
    // Authentication Status
    // ================================================================

    [HttpGet("status")]
    [AllowAnonymous]
    [ProducesResponseType(
        typeof(AuthStatusResponseDto),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<AuthStatusResponseDto>>
        GetAuthStatus(
            CancellationToken cancellationToken)
    {
        var response =
            await _authenticationService
                .GetAuthStatusAsync(
                    cancellationToken);

        return Ok(response);
    }
}

