using UAMS.Application.DTOs.Authentication.Requests;
using UAMS.Application.DTOs.Authentication.Responses;
using UAMS.Application.Interfaces.Repositories;
using UAMS.Application.Interfaces.Services;

namespace UAMS.Application.Services;

public sealed class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;
    private readonly ICurrentUserService _currentUserService;

    public AuthenticationService(
        IUserRepository userRepository,
        IPasswordService passwordService,
        ITokenService tokenService,
        ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _tokenService = tokenService;
        _currentUserService = currentUserService;
    }

    public async Task<LoginResponseDto> LoginAsync(
        LoginRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var user = await _userRepository.GetByUsernameAsync(
            request.Username,
            cancellationToken);

        if (user is null)
        {
            user = await _userRepository.GetByEmailAsync(
                request.Username,
                cancellationToken);
        }

        if (user is null)
        {
            throw new UnauthorizedAccessException(
                "Invalid username/email or password.");
        }

        if (!user.IsActive)
        {
            throw new UnauthorizedAccessException(
                "The user account is inactive.");
        }

        if (!_passwordService.VerifyPassword(
                request.Password,
                user.PasswordHash))
        {
            throw new UnauthorizedAccessException(
                "Invalid username/email or password.");
        }

        var tokens = await _tokenService.GenerateTokensAsync(
            user,
            cancellationToken);

        return new LoginResponseDto
        {
            AccessToken = tokens.AccessToken,
            RefreshToken = tokens.RefreshToken,
            ExpiresAt = tokens.ExpiresAt,
            User = new CurrentUserResponseDto
            {
                Id = user.Id,
                EmployeeId = user.EmployeeId,
                FullName = user.FullName,
                Username = user.Username,
                Email = user.Email,
                DepartmentId = user.DepartmentId
            }
        };
    }

    public async Task<LogoutResponseDto> LogoutAsync(
        LogoutRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (!string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            await _tokenService.RevokeRefreshTokenAsync(
                request.RefreshToken,
                cancellationToken);
        }

        return new LogoutResponseDto
        {
            Success = true,
            Message = "Logout successful."
        };
    }

    public async Task<RefreshTokenResponseDto> RefreshTokenAsync(
        RefreshTokenRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var tokens = await _tokenService.RefreshTokenAsync(
            request.RefreshToken,
            cancellationToken);

        if (tokens is null)
        {
            throw new UnauthorizedAccessException(
                "Invalid or expired refresh token.");
        }

        return new RefreshTokenResponseDto
        {
            AccessToken = tokens.AccessToken,
            RefreshToken = tokens.RefreshToken,
            ExpiresAt = tokens.ExpiresAt
        };
    }

    public async Task<AuthenticationResponseDto> RegisterAsync(
        RegisterRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var existingUsername =
            await _userRepository.GetByUsernameAsync(
                request.Username,
                cancellationToken);

        if (existingUsername is not null)
        {
            throw new InvalidOperationException(
                "Username is already registered.");
        }

        var existingEmail =
            await _userRepository.GetByEmailAsync(
                request.Email,
                cancellationToken);

        if (existingEmail is not null)
        {
            throw new InvalidOperationException(
                "Email is already registered.");
        }

        var passwordHash =
            _passwordService.HashPassword(request.Password);

        /*
         * User creation should be performed through the repository
         * and UnitOfWork once the exact User entity/create contract
         * is wired to this service.
         */

        throw new NotImplementedException(
            "Register workflow must be connected to the User creation "
            + "repository/UnitOfWork contract.");
    }

    public async Task<ChangePasswordResponseDto> ChangePasswordAsync(
        ChangePasswordRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var userId = _currentUserService.UserId;

        if (!userId.HasValue)
        {
            throw new UnauthorizedAccessException(
                "Authentication is required.");
        }

        var user = await _userRepository.GetByIdAsync(
            userId.Value,
            cancellationToken);

        if (user is null)
        {
            throw new UnauthorizedAccessException(
                "Authenticated user could not be found.");
        }

        if (!_passwordService.VerifyPassword(
                request.CurrentPassword,
                user.PasswordHash))
        {
            throw new UnauthorizedAccessException(
                "Current password is incorrect.");
        }

        if (request.NewPassword != request.ConfirmPassword)
        {
            throw new InvalidOperationException(
                "New password and confirmation password do not match.");
        }

        user.PasswordHash =
            _passwordService.HashPassword(request.NewPassword);

        await _userRepository.UpdateAsync(
            user,
            cancellationToken);

        return new ChangePasswordResponseDto
        {
            Success = true,
            Message = "Password changed successfully."
        };
    }

    public async Task<ForgotPasswordResponseDto> ForgotPasswordAsync(
        ForgotPasswordRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var user = await _userRepository.GetByEmailAsync(
            request.Email,
            cancellationToken);

        /*
         * Do not reveal whether the email exists.
         *
         * The real implementation should generate a short-lived
         * reset token and send it through an email service.
         */

        return new ForgotPasswordResponseDto
        {
            Success = true,
            Message =
                "If the account exists, password reset instructions "
                + "have been sent."
        };
    }

    public async Task<ResetPasswordResponseDto> ResetPasswordAsync(
        ResetPasswordRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        /*
         * Reset-token persistence/verification should be connected
         * here once the password-reset token repository/service is
         * available.
         */

        throw new NotImplementedException(
            "Password reset token verification is not yet connected.");
    }

    public async Task<VerifyEmailResponseDto> VerifyEmailAsync(
        VerifyEmailRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        /*
         * Email verification token validation should be connected
         * to the email-verification token service.
         */

        throw new NotImplementedException(
            "Email verification token validation is not yet connected.");
    }

    public async Task<VerifyEmailResponseDto> ResendVerificationEmailAsync(
        ResendVerificationEmailRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        /*
         * Generate a new verification token and send it through
         * the email service.
         */

        throw new NotImplementedException(
            "Email verification delivery service is not yet connected.");
    }

    public async Task<TokenResponseDto> RevokeRefreshTokenAsync(
        RevokeRefreshTokenRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        await _tokenService.RevokeRefreshTokenAsync(
            request.RefreshToken,
            cancellationToken);

        return new TokenResponseDto
        {
            AccessToken = string.Empty,
            RefreshToken = string.Empty,
            ExpiresAt = DateTime.UtcNow
        };
    }

    public async Task<CurrentUserResponseDto> GetCurrentUserAsync(
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        if (!userId.HasValue)
        {
            throw new UnauthorizedAccessException(
                "Authentication is required.");
        }

        var user = await _userRepository.GetByIdAsync(
            userId.Value,
            cancellationToken);

        if (user is null)
        {
            throw new UnauthorizedAccessException(
                "Authenticated user could not be found.");
        }

        return new CurrentUserResponseDto
        {
            Id = user.Id,
            EmployeeId = user.EmployeeId,
            FullName = user.FullName,
            Username = user.Username,
            Email = user.Email,
            DepartmentId = user.DepartmentId
        };
    }

    public Task<AuthStatusResponseDto> GetAuthStatusAsync(
        CancellationToken cancellationToken = default)
    {
        var response = new AuthStatusResponseDto
        {
            IsAuthenticated =
                _currentUserService.IsAuthenticated,
            UserId =
                _currentUserService.UserId,
            Username =
                _currentUserService.Username
        };

        return Task.FromResult(response);
    }
}