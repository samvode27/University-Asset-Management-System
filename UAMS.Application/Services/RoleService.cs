using UAMS.Application.DTOs.Roles.Requests;
using UAMS.Application.DTOs.Roles.Responses;
using UAMS.Application.Interfaces.Persistence;
using UAMS.Application.Interfaces.Services;

namespace UAMS.Application.Services;

public class RoleService : IRoleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public RoleService(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }


    // ================================================================
    // Create Role
    // ================================================================

    public async Task<RoleResponseDto> CreateAsync(
        CreateRoleRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var name = request.Name.Trim();
        var code = request.Code.Trim();

        var existingRole =
            await _unitOfWork.Roles.GetByNameAsync(
                name,
                cancellationToken);

        if (existingRole is not null)
        {
            throw new InvalidOperationException(
                $"A role with the name '{name}' already exists.");
        }

        var existingCode =
            await _unitOfWork.Roles.FindAsync(
                role => role.Code == code,
                cancellationToken);

        if (existingCode.Count > 0)
        {
            throw new InvalidOperationException(
                $"A role with the code '{code}' already exists.");
        }


        // ------------------------------------------------------------
        // Validate permissions
        // ------------------------------------------------------------

        var permissionIds =
            request.PermissionIds
                .Distinct()
                .ToList();

        var permissions = new List<
            UAMS.Domain.Entities.Permissions.Permission>();

        foreach (var permissionId in permissionIds)
        {
            var permission =
                await _unitOfWork.Permissions.GetByIdAsync(
                    permissionId,
                    cancellationToken);

            if (permission is null)
            {
                throw new InvalidOperationException(
                    $"Permission '{permissionId}' was not found.");
            }

            if (!permission.IsActive)
            {
                throw new InvalidOperationException(
                    $"Permission '{permission.Name}' is inactive.");
            }

            permissions.Add(permission);
        }


        // ------------------------------------------------------------
        // Create role
        // ------------------------------------------------------------

        var role =
            UAMS.Domain.Entities.Roles.Role.Create(
                name,
                code,
                request.Description,
                request.IsSystemRole);


        // ------------------------------------------------------------
        // Assign permissions
        // ------------------------------------------------------------

        var assignedBy = GetCurrentUserId();

        foreach (var permission in permissions)
        {
            role.AddPermission(
                permission.Id,
                assignedBy);
        }


        await _unitOfWork.Roles.AddAsync(
            role,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(role);
    }


    // ================================================================
    // Get Role By ID
    // ================================================================

    public async Task<RoleResponseDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Role ID is required.",
                nameof(id));
        }

        var role =
            await _unitOfWork.Roles.GetByIdAsync(
                id,
                cancellationToken);

        return role is null
            ? null
            : MapToResponse(role);
    }


    // ================================================================
    // Get Role Details
    // ================================================================

public async Task<RoleDetailResponseDto?> GetDetailsAsync(
    Guid id,
    CancellationToken cancellationToken = default)
{
    if (id == Guid.Empty)
    {
        throw new ArgumentException(
            "Role ID is required.",
            nameof(id));
    }

    var entity =
        await _unitOfWork.Roles.GetByIdWithDetailsAsync(
            id,
            cancellationToken);

    if (entity is null)
    {
        return null;
    }

    var permissions =
        entity.RolePermissions
            .Where(x => x.IsActive && x.Permission is not null)
            .Select(x => MapPermission(x.Permission))
            .ToList();

    var users =
        await _unitOfWork.Users.GetByRoleAsync(
            id,
            cancellationToken);

    return new RoleDetailResponseDto
    {
        Id = entity.Id,
        Name = entity.Name,
        Code = entity.Code,
        Description = entity.Description,
        IsSystemRole = entity.IsSystemRole,
        IsActive = entity.IsActive,
        CreatedAt = entity.CreatedAt,
        UpdatedAt = entity.UpdatedAt,
        PermissionCount = permissions.Count,
        UserCount = users.Count,
        Permissions = permissions
    };
}


    // ================================================================
    // Get Roles
    // ================================================================

    public async Task<RoleListResponseDto> GetAllAsync(
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        if (pageNumber <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(pageNumber),
                "Page number must be greater than zero.");
        }

        if (pageSize <= 0 || pageSize > 100)
        {
            throw new ArgumentOutOfRangeException(
                nameof(pageSize),
                "Page size must be between 1 and 100.");
        }


        var roles =
            await _unitOfWork.Roles.GetAllAsync(
                cancellationToken);

        var orderedRoles =
            roles
                .OrderBy(role => role.Name)
                .ToList();

        var totalCount = orderedRoles.Count;

        var totalPages =
            totalCount == 0
                ? 0
                : (int)Math.Ceiling(
                    totalCount / (double)pageSize);

        var items =
            orderedRoles
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(MapToResponse)
                .ToList();

        return new RoleListResponseDto
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = totalPages,
            HasPreviousPage = pageNumber > 1,
            HasNextPage = pageNumber < totalPages
        };
    }


    // ================================================================
    // Update Role
    // ================================================================

    public async Task<RoleResponseDto> UpdateAsync(
        Guid id,
        UpdateRoleRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Role ID is required.",
                nameof(id));
        }

        var role =
            await _unitOfWork.Roles.GetByIdAsync(
                id,
                cancellationToken);

        if (role is null)
        {
            throw new KeyNotFoundException(
                $"Role '{id}' was not found.");
        }


        // ------------------------------------------------------------
        // Check name uniqueness
        // ------------------------------------------------------------

        var existingName =
            await _unitOfWork.Roles.GetByNameAsync(
                request.Name.Trim(),
                cancellationToken);

        if (existingName is not null &&
            existingName.Id != id)
        {
            throw new InvalidOperationException(
                $"A role with the name '{request.Name}' already exists.");
        }


        // ------------------------------------------------------------
        // Check code uniqueness
        // ------------------------------------------------------------

        var existingCode =
            await _unitOfWork.Roles.FindAsync(
                roleEntity =>
                    roleEntity.Code == request.Code.Trim() &&
                    roleEntity.Id != id,
                cancellationToken);

        if (existingCode.Count > 0)
        {
            throw new InvalidOperationException(
                $"A role with the code '{request.Code}' already exists.");
        }


        // ------------------------------------------------------------
        // Update role
        // ------------------------------------------------------------

        role.Update(
            request.Name,
            request.Code,
            request.Description);

        if (request.IsActive)
        {
            role.Activate();
        }
        else
        {
            role.Deactivate();
        }

        _unitOfWork.Roles.Update(role);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(role);
    }


    // ================================================================
    // Assign Permissions
    // ================================================================

    public async Task AssignPermissionsAsync(
        Guid id,
        AssignPermissionsRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Role ID is required.",
                nameof(id));
        }

        var role =
            await _unitOfWork.Roles.GetByIdAsync(
                id,
                cancellationToken);

        if (role is null)
        {
            throw new KeyNotFoundException(
                $"Role '{id}' was not found.");
        }

        var assignedBy = GetCurrentUserId();

        foreach (var permissionId in request.PermissionIds.Distinct())
        {
            var permission =
                await _unitOfWork.Permissions.GetByIdAsync(
                    permissionId,
                    cancellationToken);

            if (permission is null)
            {
                throw new InvalidOperationException(
                    $"Permission '{permissionId}' was not found.");
            }

            if (!permission.IsActive)
            {
                throw new InvalidOperationException(
                    $"Permission '{permission.Name}' is inactive.");
            }

            role.AddPermission(
                permissionId,
                assignedBy);
        }

        _unitOfWork.Roles.Update(role);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }


    // ================================================================
    // Remove Permissions
    // ================================================================

    public async Task RemovePermissionsAsync(
        Guid id,
        RemovePermissionsRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Role ID is required.",
                nameof(id));
        }

        var role =
            await _unitOfWork.Roles.GetByIdAsync(
                id,
                cancellationToken);

        if (role is null)
        {
            throw new KeyNotFoundException(
                $"Role '{id}' was not found.");
        }

        foreach (var permissionId in request.PermissionIds.Distinct())
        {
            role.RemovePermission(permissionId);
        }

        _unitOfWork.Roles.Update(role);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }


    // ================================================================
    // Get Active Roles
    // ================================================================

    public async Task<IReadOnlyList<RoleResponseDto>>
        GetActiveRolesAsync(
            CancellationToken cancellationToken = default)
    {
        var roles =
            await _unitOfWork.Roles.GetActiveRolesAsync(
                cancellationToken);

        return roles
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Get System Roles
    // ================================================================

    public async Task<IReadOnlyList<RoleResponseDto>>
        GetSystemRolesAsync(
            CancellationToken cancellationToken = default)
    {
        var roles =
            await _unitOfWork.Roles.GetSystemRolesAsync(
                cancellationToken);

        return roles
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Mapping
    // ================================================================

    private static RoleResponseDto MapToResponse(
        UAMS.Domain.Entities.Roles.Role role)
    {
        return new RoleResponseDto
        {
            Id = role.Id,
            Name = role.Name,
            Code = role.Code,
            Description = role.Description,
            IsSystemRole = role.IsSystemRole,
            IsActive = role.IsActive,
            CreatedAt = role.CreatedAt
        };
    }


    private static PermissionResponseDto MapPermission(
        UAMS.Domain.Entities.Permissions.Permission permission)
    {
        return new PermissionResponseDto
        {
            Id = permission.Id,
            Name = permission.Name,
            Code = permission.Code,
            Description = permission.Description,
            Module = permission.Module,
            IsActive = permission.IsActive
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
                "Authenticated user is required.");
        }

        return _currentUserService.UserId.Value;
    }
}