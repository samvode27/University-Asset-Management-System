using UAMS.Application.DTOs.Users.Requests;
using UAMS.Application.DTOs.Users.Responses;
using UAMS.Application.Interfaces.Persistence;
using UAMS.Application.Interfaces.Services;
using UAMS.Domain.Entities.Users;

namespace UAMS.Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordService _passwordService;
    private readonly ICurrentUserService _currentUserService;

    public UserService(
        IUnitOfWork unitOfWork,
        IPasswordService passwordService,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _passwordService = passwordService;
        _currentUserService = currentUserService;
    }


    // ================================================================
    // Create User
    // ================================================================

    public async Task<UserDetailResponseDto> CreateUserAsync(
        CreateUserRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (await _unitOfWork.Users.ExistsByEmployeeIdAsync(
                request.EmployeeId,
                cancellationToken))
        {
            throw new InvalidOperationException(
                "A user with the specified employee ID already exists.");
        }

        if (await _unitOfWork.Users.ExistsByUsernameAsync(
                request.Username,
                cancellationToken))
        {
            throw new InvalidOperationException(
                "A user with the specified username already exists.");
        }

        if (await _unitOfWork.Users.ExistsByEmailAsync(
                request.Email,
                cancellationToken))
        {
            throw new InvalidOperationException(
                "A user with the specified email already exists.");
        }


        // ------------------------------------------------------------
        // Validate Department
        // ------------------------------------------------------------

        var department =
            await _unitOfWork.Departments.GetByIdAsync(
                request.DepartmentId,
                cancellationToken);

        if (department is null)
        {
            throw new KeyNotFoundException(
                "The specified department was not found.");
        }


        // ------------------------------------------------------------
        // Validate Role
        // ------------------------------------------------------------

        var role =
            await _unitOfWork.Roles.GetByIdAsync(
                request.RoleId,
                cancellationToken);

        if (role is null)
        {
            throw new KeyNotFoundException(
                "The specified role was not found.");
        }

        if (!role.IsActive)
        {
            throw new InvalidOperationException(
                "The specified role is inactive.");
        }


        // ------------------------------------------------------------
        // Create User
        // ------------------------------------------------------------

        var passwordHash =
            _passwordService.HashPassword(request.Password);

        var user = User.Create(
            request.EmployeeId,
            request.FullName,
            request.Email,
            request.PhoneNumber,
            request.DepartmentId,
            request.Username,
            passwordHash);


        // ------------------------------------------------------------
        // Create Initial Role Assignment
        // ------------------------------------------------------------

        var assignedBy = GetCurrentUserId();

        var userRole = UserRole.Create(
            user.Id,
            request.RoleId,
            assignedBy);

        user.UserRoles.Add(userRole);


        // ------------------------------------------------------------
        // Add User
        // ------------------------------------------------------------

        await _unitOfWork.Users.AddAsync(
            user,
            cancellationToken);


        // ------------------------------------------------------------
        // Save User + UserRole Together
        // ------------------------------------------------------------

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);


        // ------------------------------------------------------------
        // Reload User With Details
        // ------------------------------------------------------------

        var createdUser =
            await _unitOfWork.Users.GetByIdWithDetailsAsync(
                user.Id,
                cancellationToken);

        if (createdUser is null)
        {
            throw new InvalidOperationException(
                "The user was created but could not be retrieved.");
        }

        return MapToDetailResponse(createdUser);
    }


    // ================================================================
    // Get User By ID
    // ================================================================

    public async Task<UserDetailResponseDto> GetUserByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "User ID is required.",
                nameof(id));
        }

        var user =
            await _unitOfWork.Users.GetByIdWithDetailsAsync(
                id,
                cancellationToken);

        if (user is null)
        {
            throw new KeyNotFoundException(
                "User not found.");
        }

        return MapToDetailResponse(user);
    }


    // ================================================================
    // Get Users
    // ================================================================

    public async Task<UserListResponseDto> GetUsersAsync(
        UserFilterRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var users =
            await _unitOfWork.Users.GetAllAsync(
                cancellationToken);

        IEnumerable<User> query = users;


        // ------------------------------------------------------------
        // Search
        // ------------------------------------------------------------

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search =
                request.Search.Trim();

            query = query.Where(user =>
                user.EmployeeId.Contains(
                    search,
                    StringComparison.OrdinalIgnoreCase)
                ||
                user.FullName.Contains(
                    search,
                    StringComparison.OrdinalIgnoreCase)
                ||
                user.Email.Contains(
                    search,
                    StringComparison.OrdinalIgnoreCase)
                ||
                user.Username.Contains(
                    search,
                    StringComparison.OrdinalIgnoreCase));
        }


        // ------------------------------------------------------------
        // Department
        // ------------------------------------------------------------

        if (request.DepartmentId.HasValue)
        {
            query = query.Where(
                user =>
                    user.DepartmentId ==
                    request.DepartmentId.Value);
        }


        // ------------------------------------------------------------
        // Active Status
        // ------------------------------------------------------------

        if (request.IsActive.HasValue)
        {
            query = query.Where(
                user =>
                    user.IsActive ==
                    request.IsActive.Value);
        }


        // ------------------------------------------------------------
        // Locked Status
        // ------------------------------------------------------------

        if (request.IsLocked.HasValue)
        {
            query = query.Where(
                user =>
                    user.IsLocked ==
                    request.IsLocked.Value);
        }


        // ------------------------------------------------------------
        // Deleted Status
        // ------------------------------------------------------------

        if (request.IsDeleted.HasValue)
        {
            query = query.Where(
                user =>
                    user.IsDeleted ==
                    request.IsDeleted.Value);
        }


        // ------------------------------------------------------------
        // Created Date Range
        // ------------------------------------------------------------

        if (request.CreatedFrom.HasValue)
        {
            query = query.Where(
                user =>
                    user.CreatedAt >=
                    request.CreatedFrom.Value);
        }

        if (request.CreatedTo.HasValue)
        {
            query = query.Where(
                user =>
                    user.CreatedAt <=
                    request.CreatedTo.Value);
        }


        // ------------------------------------------------------------
        // Role Filter
        // ------------------------------------------------------------

        if (request.RoleId.HasValue)
        {
            var usersByRole =
                await _unitOfWork.Users.GetByRoleAsync(
                    request.RoleId.Value,
                    cancellationToken);

            var roleUserIds =
                usersByRole
                    .Select(user => user.Id)
                    .ToHashSet();

            query = query.Where(
                user =>
                    roleUserIds.Contains(user.Id));
        }


        // ------------------------------------------------------------
        // Sorting
        // ------------------------------------------------------------

        query = request.SortBy?.Trim().ToLowerInvariant() switch
        {
            "employeeid" =>
                request.SortDescending
                    ? query.OrderByDescending(user => user.EmployeeId)
                    : query.OrderBy(user => user.EmployeeId),

            "fullname" =>
                request.SortDescending
                    ? query.OrderByDescending(user => user.FullName)
                    : query.OrderBy(user => user.FullName),

            "email" =>
                request.SortDescending
                    ? query.OrderByDescending(user => user.Email)
                    : query.OrderBy(user => user.Email),

            "username" =>
                request.SortDescending
                    ? query.OrderByDescending(user => user.Username)
                    : query.OrderBy(user => user.Username),

            "createdat" =>
                request.SortDescending
                    ? query.OrderByDescending(user => user.CreatedAt)
                    : query.OrderBy(user => user.CreatedAt),

            _ =>
                query.OrderBy(user => user.FullName)
        };


        // ------------------------------------------------------------
        // Total Count
        // ------------------------------------------------------------

        var totalCount =
            query.Count();


        // ------------------------------------------------------------
        // Pagination
        // ------------------------------------------------------------

        var pageNumber =
            request.PageNumber;

        var pageSize =
            request.PageSize;

        var items =
            query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(MapToResponse)
                .ToList();


        var totalPages =
            totalCount == 0
                ? 0
                : (int)Math.Ceiling(
                    totalCount /
                    (double)pageSize);


        return new UserListResponseDto
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            HasPreviousPage = pageNumber > 1,
            HasNextPage = pageNumber < totalPages
        };
    }


    // ================================================================
    // Update User
    // ================================================================

    public async Task<UserDetailResponseDto> UpdateUserAsync(
        Guid id,
        UpdateUserRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "User ID is required.",
                nameof(id));
        }

        var user =
            await _unitOfWork.Users.GetByIdAsync(
                id,
                cancellationToken);

        if (user is null || user.IsDeleted)
        {
            throw new KeyNotFoundException(
                "User not found.");
        }


        // ------------------------------------------------------------
        // Check Email Uniqueness
        // ------------------------------------------------------------

        var existingEmail =
            await _unitOfWork.Users.GetByEmailAsync(
                request.Email,
                cancellationToken);

        if (existingEmail is not null &&
            existingEmail.Id != id)
        {
            throw new InvalidOperationException(
                "A user with the specified email already exists.");
        }


        // ------------------------------------------------------------
        // Update Profile
        // ------------------------------------------------------------

        user.UpdateProfile(
            request.FullName,
            request.Email,
            request.PhoneNumber);


        // ------------------------------------------------------------
        // Department
        // ------------------------------------------------------------

        if (user.DepartmentId != request.DepartmentId)
        {
            var department =
                await _unitOfWork.Departments.GetByIdAsync(
                    request.DepartmentId,
                    cancellationToken);

            if (department is null)
            {
                throw new KeyNotFoundException(
                    "The specified department was not found.");
            }

            user.ChangeDepartment(
                request.DepartmentId);
        }


        // ------------------------------------------------------------
        // Active Status
        // ------------------------------------------------------------

        if (request.IsActive)
        {
            user.Activate();
        }
        else
        {
            user.Deactivate();
        }


        await _unitOfWork.SaveChangesAsync(
            cancellationToken);


        var updatedUser =
            await _unitOfWork.Users.GetByIdWithDetailsAsync(
                id,
                cancellationToken);

        if (updatedUser is null)
        {
            throw new InvalidOperationException(
                "The user was updated but could not be retrieved.");
        }

        return MapToDetailResponse(updatedUser);
    }


    // ================================================================
    // Reset User Password
    // ================================================================

    public async Task ResetUserPasswordAsync(
        Guid id,
        ResetUserPasswordRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "User ID is required.",
                nameof(id));
        }

        var user =
            await _unitOfWork.Users.GetByIdWithAuthenticationDataAsync(
                id,
                cancellationToken);

        if (user is null)
        {
            throw new KeyNotFoundException(
                "User not found.");
        }

        var passwordHash =
            _passwordService.HashPassword(
                request.NewPassword);

        user.ResetPassword(
            passwordHash);

        _unitOfWork.Users.Update(user);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }


    // ================================================================
    // Assign Role
    // ================================================================

    public async Task AssignRoleAsync(
        Guid id,
        AssignRoleRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "User ID is required.",
                nameof(id));
        }

        var user =
            await _unitOfWork.Users.GetByIdWithDetailsAsync(
                id,
                cancellationToken);

        if (user is null)
        {
            throw new KeyNotFoundException(
                "User not found.");
        }


        var role =
            await _unitOfWork.Roles.GetByIdAsync(
                request.RoleId,
                cancellationToken);

        if (role is null)
        {
            throw new KeyNotFoundException(
                "The specified role was not found.");
        }

        if (!role.IsActive)
        {
            throw new InvalidOperationException(
                "The specified role is inactive.");
        }


        var existingRole =
            user.UserRoles.FirstOrDefault(
                userRole =>
                    userRole.RoleId == request.RoleId &&
                    userRole.IsActive);

        if (existingRole is not null)
        {
            throw new InvalidOperationException(
                "The user already has this role.");
        }


        var assignedBy =
            GetCurrentUserId();

        var userRole =
            UserRole.Create(
                id,
                request.RoleId,
                assignedBy);


        // ------------------------------------------------------------
        // Add UserRole directly
        // ------------------------------------------------------------

        await _unitOfWork.UserRoles.AddAsync(
            userRole,
            cancellationToken);


        // ------------------------------------------------------------
        // Save
        // ------------------------------------------------------------

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }


    // ================================================================
    // Change Department
    // ================================================================

    public async Task ChangeDepartmentAsync(
        Guid id,
        ChangeDepartmentRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "User ID is required.",
                nameof(id));
        }

        var user =
            await _unitOfWork.Users.GetByIdAsync(
                id,
                cancellationToken);

        if (user is null)
        {
            throw new KeyNotFoundException(
                "User not found.");
        }


        var department =
            await _unitOfWork.Departments.GetByIdAsync(
                request.DepartmentId,
                cancellationToken);

        if (department is null)
        {
            throw new KeyNotFoundException(
                "The specified department was not found.");
        }


        user.ChangeDepartment(
            request.DepartmentId);

        _unitOfWork.Users.Update(user);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }


    // ================================================================
    // Activate User
    // ================================================================

    public async Task ActivateUserAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "User ID is required.",
                nameof(id));
        }

        var user =
            await _unitOfWork.Users.GetByIdAsync(
                id,
                cancellationToken);

        if (user is null)
        {
            throw new KeyNotFoundException(
                "User not found.");
        }

        user.Activate();

        _unitOfWork.Users.Update(user);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }


    // ================================================================
    // Deactivate User
    // ================================================================

    public async Task DeactivateUserAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "User ID is required.",
                nameof(id));
        }

        var user =
            await _unitOfWork.Users.GetByIdAsync(
                id,
                cancellationToken);

        if (user is null)
        {
            throw new KeyNotFoundException(
                "User not found.");
        }

        user.Deactivate();

        _unitOfWork.Users.Update(user);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }

    // ================================================================
    // Delete User
    // ================================================================

    public async Task DeleteUserAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "User ID is required.",
                nameof(id));
        }

        var user =
            await _unitOfWork.Users.GetByIdAsync(
                id,
                cancellationToken);

        if (user is null)
        {
            throw new KeyNotFoundException(
                "User not found.");
        }


        var deletedBy =
            GetCurrentUserId();

        user.SoftDelete(
            deletedBy);

        _unitOfWork.Users.Update(user);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }


    // ================================================================
    // Restore User
    // ================================================================

    public async Task RestoreAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "User ID is required.",
                nameof(id));
        }

        var user =
            await _unitOfWork.Users.GetDeletedByIdAsync(
                id,
                cancellationToken);

        if (user is null)
        {
            throw new KeyNotFoundException(
                "User not found.");
        }


        user.Restore();

        _unitOfWork.Users.Update(user);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }


    // ================================================================
    // Unlock User
    // ================================================================

    public async Task UnlockAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "User ID is required.",
                nameof(id));
        }

        var user =
            await _unitOfWork.Users.GetByIdAsync(
                id,
                cancellationToken);

        if (user is null)
        {
            throw new KeyNotFoundException(
                "User not found.");
        }

        user.UnlockAccount();

        _unitOfWork.Users.Update(user);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }


    // ================================================================
    // Mapping
    // ================================================================

    private static UserResponseDto MapToResponse(
        User user)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            EmployeeId = user.EmployeeId,
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            DepartmentId = user.DepartmentId,
            DepartmentName =
                user.Department?.Name ?? string.Empty,
            Username = user.Username,
            IsActive = user.IsActive,
            IsLocked = user.IsLocked,
            LastLoginAt = user.LastLoginAt,
            CreatedAt = user.CreatedAt
        };
    }


    private static UserDetailResponseDto MapToDetailResponse(
        User user)
    {
        return new UserDetailResponseDto
        {
            Id = user.Id,
            EmployeeId = user.EmployeeId,
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            DepartmentId = user.DepartmentId,
            DepartmentName =
                user.Department?.Name ?? string.Empty,
            Username = user.Username,
            IsActive = user.IsActive,
            IsLocked = user.IsLocked,
            FailedLoginAttempts =
                user.FailedLoginAttempts,
            LastLoginAt =
                user.LastLoginAt,
            LockedAt =
                user.LockedAt,
            CreatedAt =
                user.CreatedAt,
            UpdatedAt =
                user.UpdatedAt,
            Roles =
                user.UserRoles
                    .Where(userRole =>
                        userRole.IsActive &&
                        userRole.Role != null)
                    .Select(userRole =>
                        new UserRoleResponseDto
                        {
                            RoleId =
                                userRole.RoleId,
                            RoleName =
                                userRole.Role.Name
                        })
                    .ToList()
        };
    }


    // ================================================================
    // Current User
    // ================================================================

    private Guid GetCurrentUserId()
    {
        if (!_currentUserService.IsAuthenticated ||
            !_currentUserService.UserId.HasValue ||
            _currentUserService.UserId.Value == Guid.Empty)
        {
            throw new UnauthorizedAccessException(
                "The current user is not authenticated.");
        }

        return _currentUserService.UserId.Value;
    }
}