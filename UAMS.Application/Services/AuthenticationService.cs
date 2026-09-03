using UAMS.Application.DTOs.Authentication.Requests;
using UAMS.Application.DTOs.Authentication.Responses;
using UAMS.Application.Interfaces.Repositories;
using UAMS.Application.Interfaces.Services;
using UAMS.Domain.Entities.Users;

using Microsoft.Extensions.Options;
using UAMS.Application.Options;
using UAMS.Application.Interfaces.Persistence;

namespace UAMS.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;
    private readonly ICurrentUserService _currentUserService;
    private readonly AuthenticationOptions _authenticationOptions;

    public AuthenticationService(
     IUserRepository userRepository,
     IPasswordService passwordService,
     ITokenService tokenService,
     ICurrentUserService currentUserService,
     IUnitOfWork unitOfWork,
     IOptions<AuthenticationOptions> authenticationOptions)
    {
        _userRepository =
            userRepository
            ?? throw new ArgumentNullException(nameof(userRepository));

        _passwordService =
            passwordService
            ?? throw new ArgumentNullException(nameof(passwordService));

        _tokenService =
            tokenService
            ?? throw new ArgumentNullException(nameof(tokenService));

        _currentUserService =
            currentUserService
            ?? throw new ArgumentNullException(nameof(currentUserService));

        _unitOfWork =
            unitOfWork
            ?? throw new ArgumentNullException(nameof(unitOfWork));

        _authenticationOptions =
            authenticationOptions?.Value
            ?? throw new ArgumentNullException(nameof(authenticationOptions));
    }


    // ================================================================
    // Login
    // ================================================================

    public async Task<LoginResponseDto> LoginAsync(
        LoginRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var identifier =
            request.UsernameOrEmail?.Trim();

        if (string.IsNullOrWhiteSpace(identifier))
        {
            return new LoginResponseDto
            {
                Succeeded = false,
                Message = "Username or email is required."
            };
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            return new LoginResponseDto
            {
                Succeeded = false,
                Message = "Password is required."
            };
        }


        // ============================================================
        // Normalize Login Identifier
        // ============================================================

        User? user;

        if (identifier.Contains('@'))
        {
            user =
                await _userRepository
                    .GetByEmailWithAuthenticationDataAsync(
                        identifier.ToLowerInvariant(),
                        cancellationToken);
        }
        else
        {
            user =
                await _userRepository
                    .GetByUsernameWithAuthenticationDataAsync(
                        identifier,
                        cancellationToken);
        }


        // ============================================================
        // Invalid Credentials
        // ============================================================

        if (user is null)
        {
            return new LoginResponseDto
            {
                Succeeded = false,
                Message = "Invalid username/email or password."
            };
        }


        // ============================================================
        // Account Status
        // ============================================================

        if (!user.IsActive)
        {
            return new LoginResponseDto
            {
                Succeeded = false,
                Message = "This account is inactive."
            };
        }


        // ============================================================
        // Account Lock
        // ============================================================

        if (user.IsLocked)
        {
            return new LoginResponseDto
            {
                Succeeded = false,
                Message = "This account is locked."
            };
        }


        // ============================================================
        // Password Verification
        // ============================================================

        var passwordValid =
            _passwordService.VerifyPassword(
                request.Password,
                user.PasswordHash);

        if (!passwordValid)
        {
            user.RecordFailedLogin(
                _authenticationOptions.MaxFailedLoginAttempts);

            _userRepository.Update(user);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            if (user.IsLocked)
            {
                return new LoginResponseDto
                {
                    Succeeded = false,
                    Message =
                        "Your account has been locked because of too many failed login attempts."
                };
            }

            return new LoginResponseDto
            {
                Succeeded = false,
                Message = "Invalid username/email or password."
            };
        }


        // ============================================================
        // Successful Login
        // ============================================================

        var loginAt = DateTime.UtcNow;

        user.RecordSuccessfulLogin(loginAt);

        _userRepository.Update(user);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);


        // ============================================================
        // Generate Tokens
        // ============================================================

        var tokens =
            await _tokenService.GenerateTokensAsync(
                user,
                request.RememberMe,
                cancellationToken);


        // ============================================================
        // Current User
        // ============================================================

        var currentUser =
            MapCurrentUser(user);


        // ============================================================
        // Session
        //
        // NOTE:
        // This is currently a response-level session representation.
        // Persistent session/refresh-token tracking will be implemented
        // in Phase B.
        // ============================================================

        var session = new UserSessionResponseDto
        {
            SessionId = Guid.NewGuid(),
            UserId = user.Id,
            LoginAt = loginAt,
            LastActivityAt = loginAt,
            ExpiresAt = tokens.RefreshTokenExpiresAt,
            IsActive = true
        };


        // ============================================================
        // Response
        // ============================================================

        return new LoginResponseDto
        {
            Succeeded = true,
            Message = "Login successful.",
            Tokens = tokens,
            User = currentUser,
            Session = session
        };
    }


    // ================================================================
    // Logout
    // ================================================================

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
            Succeeded = true,
            Message = "Logout successful.",
            LoggedOutAt = DateTime.UtcNow
        };
    }


    // ================================================================
    // Refresh Token
    // ================================================================

    public async Task<RefreshTokenResponseDto> RefreshTokenAsync(
        RefreshTokenRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            return new RefreshTokenResponseDto
            {
                Succeeded = false,
                Message = "Refresh token is required."
            };
        }


        var tokens =
            await _tokenService.RefreshTokenAsync(
                request.RefreshToken,
                cancellationToken);


        if (tokens is null)
        {
            return new RefreshTokenResponseDto
            {
                Succeeded = false,
                Message = "Invalid or expired refresh token."
            };
        }


        return new RefreshTokenResponseDto
        {
            Succeeded = true,
            Message = "Token refreshed successfully.",
            Tokens = tokens
        };
    }


    // ================================================================
    // Register
    // ================================================================

    public async Task<AuthenticationResponseDto> RegisterAsync(
        RegisterRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);


        // ============================================================
        // Normalize Input
        // ============================================================

        var employeeId =
            request.EmployeeId?.Trim();

        var fullName =
            request.FullName?.Trim();

        var email =
            request.Email?.Trim().ToLowerInvariant();

        var phoneNumber =
            request.PhoneNumber?.Trim();

        var username =
            request.Username?.Trim();


        // ============================================================
        // Required Values
        //
        // FluentValidation normally handles these checks before the
        // service is called. These checks keep the service safe when
        // invoked directly.
        // ============================================================

        if (string.IsNullOrWhiteSpace(employeeId) ||
            string.IsNullOrWhiteSpace(fullName) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(phoneNumber) ||
            string.IsNullOrWhiteSpace(username))
        {
            return new AuthenticationResponseDto
            {
                Succeeded = false,
                Message = "Required registration information is missing."
            };
        }


        // ============================================================
        // Password Confirmation
        // ============================================================

        if (request.Password != request.ConfirmPassword)
        {
            return new AuthenticationResponseDto
            {
                Succeeded = false,
                Message = "Passwords do not match."
            };
        }


        // ============================================================
        // Username Uniqueness
        // ============================================================

        if (await _userRepository.ExistsByUsernameAsync(
        username,
        cancellationToken))
        {
            return new AuthenticationResponseDto
            {
                Succeeded = false,
                Message = "Username already exists."
            };
        }


        // ============================================================
        // Email Uniqueness
        // ============================================================

        if (await _userRepository.ExistsByEmailAsync(
         email,
         cancellationToken))
        {
            return new AuthenticationResponseDto
            {
                Succeeded = false,
                Message = "Email already exists."
            };
        }


        // ============================================================
        // Employee ID Uniqueness
        // ============================================================

        if (await _userRepository.ExistsByEmployeeIdAsync(
        employeeId,
        cancellationToken))
        {
            return new AuthenticationResponseDto
            {
                Succeeded = false,
                Message = "Employee ID already exists."
            };
        }


        // ============================================================
        // Department Validation
        // ============================================================

        var department =
            await _unitOfWork.Departments.GetByIdAsync(
                request.DepartmentId,
                cancellationToken);

        if (department is null)
        {
            return new AuthenticationResponseDto
            {
                Succeeded = false,
                Message = "The selected department does not exist."
            };
        }


        // ============================================================
        // Password Hashing
        // ============================================================

        var passwordHash =
            _passwordService.HashPassword(
                request.Password);


        // ============================================================
        // Create User
        // ============================================================

        var user = User.Create(
            employeeId,
            fullName,
            email,
            phoneNumber,
            request.DepartmentId,
            username,
            passwordHash);


        // ============================================================
        // Persist User
        // ============================================================

        await _userRepository.AddAsync(
            user,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);


        // ============================================================
        // Response
        //
        // Email verification will be added in Phase D.
        // Role assignment will be handled in the authorization/
        // role-resolution phase.
        // ============================================================

        return new AuthenticationResponseDto
        {
            Succeeded = true,
            Message = "User registered successfully.",
            User = MapCurrentUser(user)
        };
    }


    // ================================================================
    // Change Password
    // ================================================================

    public async Task<ChangePasswordResponseDto> ChangePasswordAsync(
        ChangePasswordRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);


        // ============================================================
        // Authentication
        // ============================================================

        if (!_currentUserService.IsAuthenticated ||
            !_currentUserService.UserId.HasValue)
        {
            return new ChangePasswordResponseDto
            {
                Succeeded = false,
                Message = "Authentication is required."
            };
        }


        // ============================================================
        // Password Confirmation
        // ============================================================

        if (request.NewPassword != request.ConfirmPassword)
        {
            return new ChangePasswordResponseDto
            {
                Succeeded = false,
                Message =
                    "New password and confirmation do not match."
            };
        }


        // ============================================================
        // Get Current User
        // ============================================================

        var user =
            await _userRepository.GetByIdAsync(
                _currentUserService.UserId.Value,
                cancellationToken);

        if (user is null)
        {
            return new ChangePasswordResponseDto
            {
                Succeeded = false,
                Message = "User account was not found."
            };
        }


        // ============================================================
        // Account Status
        // ============================================================

        if (!user.IsActive)
        {
            return new ChangePasswordResponseDto
            {
                Succeeded = false,
                Message = "This account is inactive."
            };
        }


        if (user.IsLocked)
        {
            return new ChangePasswordResponseDto
            {
                Succeeded = false,
                Message = "This account is locked."
            };
        }


        // ============================================================
        // Verify Current Password
        // ============================================================

        var currentPasswordValid =
            _passwordService.VerifyPassword(
                request.CurrentPassword,
                user.PasswordHash);

        if (!currentPasswordValid)
        {
            return new ChangePasswordResponseDto
            {
                Succeeded = false,
                Message = "Current password is incorrect."
            };
        }


        // ============================================================
        // Prevent Reusing Current Password
        // ============================================================

        var samePassword =
            _passwordService.VerifyPassword(
                request.NewPassword,
                user.PasswordHash);

        if (samePassword)
        {
            return new ChangePasswordResponseDto
            {
                Succeeded = false,
                Message =
                    "The new password must be different from the current password."
            };
        }


        // ============================================================
        // Hash New Password
        // ============================================================

        var newPasswordHash =
            _passwordService.HashPassword(
                request.NewPassword);


        // ============================================================
        // Change Password
        // ============================================================

        user.ChangePassword(
            newPasswordHash);


        // ============================================================
        // Persist Changes
        // ============================================================

        _userRepository.Update(user);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);


        // ============================================================
        // Response
        // ============================================================

        return new ChangePasswordResponseDto
        {
            Succeeded = true,
            Message = "Password changed successfully.",
            ChangedAt = DateTime.UtcNow
        };
    }


    // ================================================================
    // Forgot Password
    // ================================================================

    public async Task<ForgotPasswordResponseDto> ForgotPasswordAsync(
        ForgotPasswordRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        /*
         * Do not reveal whether an email exists.
         */

        return new ForgotPasswordResponseDto
        {
            Succeeded = true,
            Message =
                "If the email is registered, password reset instructions " +
                "will be sent.",
            RequestedAt = DateTime.UtcNow
        };
    }


    // ================================================================
    // Reset Password
    // ================================================================

    public async Task<ResetPasswordResponseDto> ResetPasswordAsync(
        ResetPasswordRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);


        if (request.NewPassword != request.ConfirmPassword)
        {
            return new ResetPasswordResponseDto
            {
                Succeeded = false,
                Message = "Passwords do not match.",
                ResetAt = DateTime.UtcNow
            };
        }


        /*
         * Password reset token persistence/validation is not yet
         * available in the current domain model.
         */

        return new ResetPasswordResponseDto
        {
            Succeeded = false,
            Message =
                "Password reset token infrastructure has not yet " +
                "been configured.",
            ResetAt = DateTime.UtcNow
        };
    }


    // ================================================================
    // Verify Email
    // ================================================================

    public Task<VerifyEmailResponseDto> VerifyEmailAsync(
        VerifyEmailRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        return Task.FromResult(
            new VerifyEmailResponseDto
            {
                Succeeded = false,
                Message =
                    "Email verification token infrastructure has not " +
                    "yet been configured.",
                VerifiedAt = DateTime.UtcNow
            });
    }


    // ================================================================
    // Resend Verification Email
    // ================================================================

    public async Task<VerifyEmailResponseDto>
        ResendVerificationEmailAsync(
            ResendVerificationEmailRequestDto request,
            CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        return new VerifyEmailResponseDto
        {
            Succeeded = true,
            Message =
                "If the email is registered, a verification email " +
                "will be sent.",
            VerifiedAt = DateTime.UtcNow
        };
    }


    // ================================================================
    // Revoke Refresh Token
    // ================================================================

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
            AccessTokenExpiresAt = DateTime.UtcNow,
            RefreshTokenExpiresAt = DateTime.UtcNow
        };
    }


    // ================================================================
    // Current User
    // ================================================================

    public async Task<CurrentUserResponseDto>
        GetCurrentUserAsync(
            CancellationToken cancellationToken = default)
    {
        // ============================================================
        // Authentication
        // ============================================================

        if (!_currentUserService.IsAuthenticated ||
            !_currentUserService.UserId.HasValue)
        {
            throw new UnauthorizedAccessException(
                "Authentication is required.");
        }


        // ============================================================
        // Get Current User With Authentication Data
        // ============================================================

        var user =
            await _userRepository
                .GetByIdWithAuthenticationDataAsync(
                    _currentUserService.UserId.Value,
                    cancellationToken);


        // ============================================================
        // User Not Found
        // ============================================================

        if (user is null)
        {
            throw new UnauthorizedAccessException(
                "User account was not found.");
        }


        // ============================================================
        // Account Status
        // ============================================================

        if (!user.IsActive)
        {
            throw new UnauthorizedAccessException(
                "This account is inactive.");
        }


        if (user.IsLocked)
        {
            throw new UnauthorizedAccessException(
                "This account is locked.");
        }


        // ============================================================
        // Map Current User
        // ============================================================

        return MapCurrentUser(user);
    }


    // ================================================================
    // Authentication Status
    // ================================================================

    public async Task<AuthStatusResponseDto> GetAuthStatusAsync(
        CancellationToken cancellationToken = default)
    {
        if (!_currentUserService.IsAuthenticated ||
            !_currentUserService.UserId.HasValue)
        {
            return new AuthStatusResponseDto
            {
                IsAuthenticated = false,
                IsActive = false,
                IsLocked = false,
                User = null
            };
        }

        var user =
            await _userRepository.GetByIdWithAuthenticationDataAsync(
                _currentUserService.UserId.Value,
                cancellationToken);

        if (user is null)
        {
            return new AuthStatusResponseDto
            {
                IsAuthenticated = false,
                IsActive = false,
                IsLocked = false,
                User = null
            };
        }

        return new AuthStatusResponseDto
        {
            IsAuthenticated = true,
            IsActive = user.IsActive,
            IsLocked = user.IsLocked,
            User = MapCurrentUser(user)
        };
    }


    // ================================================================
    // Authorization Resolution
    // ================================================================

    private static (
        List<string> Roles,
        List<string> Permissions)
        ResolveAuthorizationData(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        var activeRoles = user.UserRoles
            .Where(userRole =>
                userRole.IsActive &&
                userRole.Role != null)
            .Select(userRole => userRole.Role)
            .Distinct()
            .ToList();

        var roles = activeRoles
            .Where(role =>
                role.IsActive &&
                !role.IsDeleted)
            .Select(role => role.Name)
            .Where(name =>
                !string.IsNullOrWhiteSpace(name))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(name => name)
            .ToList();

        var permissions = activeRoles
            .Where(role =>
                role.IsActive &&
                !role.IsDeleted)
            .SelectMany(role => role.RolePermissions)
            .Where(rolePermission =>
                rolePermission.IsActive &&
                rolePermission.Permission != null &&
                rolePermission.Permission.IsActive &&
                !rolePermission.Permission.IsDeleted)
            .Select(rolePermission =>
                rolePermission.Permission.Code)
            .Where(code =>
                !string.IsNullOrWhiteSpace(code))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(code => code)
            .ToList();

        return (roles, permissions);
    }


    // ================================================================
    // Mapping
    // ================================================================

    private static CurrentUserResponseDto MapCurrentUser(
        User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        var authorization =
            ResolveAuthorizationData(user);

        return new CurrentUserResponseDto
        {
            Id = user.Id,
            EmployeeId = user.EmployeeId,
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Username = user.Username,
            DepartmentId = user.DepartmentId,
            DepartmentName =
                user.Department?.Name ?? string.Empty,

            Roles = authorization.Roles,

            Permissions = authorization.Permissions,

            IsActive = user.IsActive
        };
    }


}