using UAMS.Application.DTOs.Permission.Requests;
using UAMS.Application.DTOs.Permission.Responses;
using UAMS.Application.Interfaces.Persistence;
using UAMS.Application.Interfaces.Services;
using UAMS.Domain.Entities.Permissions;

namespace UAMS.Application.Services;

public class PermissionService : IPermissionService
{
    private readonly IUnitOfWork _unitOfWork;

    public PermissionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork
            ?? throw new ArgumentNullException(nameof(unitOfWork));
    }


    // ============================================================
    // Create
    // ============================================================

    public async Task<PermissionResponseDto> CreateAsync(
        CreatePermissionRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        // --------------------------------------------------------
        // Validate Name
        // --------------------------------------------------------

        ArgumentException.ThrowIfNullOrWhiteSpace(
            request.Name,
            nameof(request.Name));

        // --------------------------------------------------------
        // Validate Code
        // --------------------------------------------------------

        ArgumentException.ThrowIfNullOrWhiteSpace(
            request.Code,
            nameof(request.Code));

        // --------------------------------------------------------
        // Validate Module
        // --------------------------------------------------------

        ArgumentException.ThrowIfNullOrWhiteSpace(
            request.Module,
            nameof(request.Module));

        var name = request.Name.Trim();
        var code = request.Code.Trim();
        var module = request.Module.Trim();

        // --------------------------------------------------------
        // Check Duplicate Name
        // --------------------------------------------------------

        var existingByName =
            await _unitOfWork.Permissions.GetByNameAsync(
                name,
                cancellationToken);

        if (existingByName is not null)
        {
            throw new InvalidOperationException(
                $"A permission with the name '{name}' already exists.");
        }

        // --------------------------------------------------------
        // Check Duplicate Code
        // --------------------------------------------------------

        var codeExists =
            await _unitOfWork.Permissions.ExistsAsync(
                permission => permission.Code == code,
                cancellationToken);

        if (codeExists)
        {
            throw new InvalidOperationException(
                $"A permission with the code '{code}' already exists.");
        }

        // --------------------------------------------------------
        // Create Domain Entity
        // --------------------------------------------------------

        var permission = Permission.Create(
            name,
            code,
            request.Description,
            module,
            request.CreatedBy);

        // --------------------------------------------------------
        // Persist
        // --------------------------------------------------------

        await _unitOfWork.Permissions.AddAsync(
            permission,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(permission);
    }


    // ============================================================
    // Get By ID
    // ============================================================

    public async Task<PermissionDetailResponseDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Permission ID is required.",
                nameof(id));
        }

        var permission =
            await _unitOfWork.Permissions.GetByIdAsync(
                id,
                cancellationToken);

        if (permission is null)
        {
            return null;
        }

        return MapToDetailResponse(permission);
    }


    // ============================================================
    // Get By Name
    // ============================================================

    public async Task<PermissionResponseDto?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            name,
            nameof(name));

        var permission =
            await _unitOfWork.Permissions.GetByNameAsync(
                name.Trim(),
                cancellationToken);

        if (permission is null)
        {
            return null;
        }

        return MapToResponse(permission);
    }


    // ============================================================
    // Get By Module
    // ============================================================

    public async Task<IReadOnlyList<PermissionResponseDto>>
        GetByModuleAsync(
            string module,
            CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            module,
            nameof(module));

        var permissions =
            await _unitOfWork.Permissions.GetByModuleAsync(
                module.Trim(),
                cancellationToken);

        return permissions
            .Select(MapToResponse)
            .ToList();
    }


    // ============================================================
    // Get Active
    // ============================================================

    public async Task<IReadOnlyList<PermissionResponseDto>>
        GetActiveAsync(
            CancellationToken cancellationToken = default)
    {
        var permissions =
            await _unitOfWork.Permissions.GetActivePermissionsAsync(
                cancellationToken);

        return permissions
            .Select(MapToResponse)
            .ToList();
    }


    // ============================================================
    // Get All / Filter
    // ============================================================

    public async Task<PermissionListResponseDto> GetAllAsync(
        PermissionFilterRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var permissions =
            await _unitOfWork.Permissions.GetAllAsync(
                cancellationToken);

        IEnumerable<Permission> query = permissions;

        // --------------------------------------------------------
        // Name
        // --------------------------------------------------------

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var name = request.Name.Trim();

            query = query.Where(x =>
                x.Name.Contains(
                    name,
                    StringComparison.OrdinalIgnoreCase));
        }

        // --------------------------------------------------------
        // Code
        // --------------------------------------------------------

        if (!string.IsNullOrWhiteSpace(request.Code))
        {
            var code = request.Code.Trim();

            query = query.Where(x =>
                x.Code.Contains(
                    code,
                    StringComparison.OrdinalIgnoreCase));
        }

        // --------------------------------------------------------
        // Module
        // --------------------------------------------------------

        if (!string.IsNullOrWhiteSpace(request.Module))
        {
            var module = request.Module.Trim();

            query = query.Where(x =>
                x.Module.Contains(
                    module,
                    StringComparison.OrdinalIgnoreCase));
        }

        // --------------------------------------------------------
        // Search
        // --------------------------------------------------------

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm =
                request.SearchTerm.Trim();

            query = query.Where(x =>
                x.Name.Contains(
                    searchTerm,
                    StringComparison.OrdinalIgnoreCase)

                ||

                x.Code.Contains(
                    searchTerm,
                    StringComparison.OrdinalIgnoreCase)

                ||

                x.Module.Contains(
                    searchTerm,
                    StringComparison.OrdinalIgnoreCase)

                ||

                (
                    x.Description != null
                    &&
                    x.Description.Contains(
                        searchTerm,
                        StringComparison.OrdinalIgnoreCase)
                ));
        }

        // --------------------------------------------------------
        // Active State
        // --------------------------------------------------------

        if (request.IsActive.HasValue)
        {
            query = query.Where(x =>
                x.IsActive == request.IsActive.Value);
        }

        // --------------------------------------------------------
        // Ordering
        // --------------------------------------------------------

        query = query
            .OrderBy(x => x.Module)
            .ThenBy(x => x.Name);

        // --------------------------------------------------------
        // Pagination
        // --------------------------------------------------------

        var totalCount = query.Count();

        var pageNumber =
            request.PageNumber < 1
                ? 1
                : request.PageNumber;

        var pageSize =
            request.PageSize < 1
                ? 20
                : Math.Min(request.PageSize, 100);

        var totalPages =
            totalCount == 0
                ? 0
                : (int)Math.Ceiling(
                    totalCount / (double)pageSize);

        var items = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(MapToResponse)
            .ToList();

        return new PermissionListResponseDto
        {
            Items = items,

            TotalCount = totalCount,

            PageNumber = pageNumber,

            PageSize = pageSize,

            TotalPages = totalPages
        };
    }


    // ============================================================
    // Update
    // ============================================================

    public async Task<PermissionResponseDto> UpdateAsync(
        Guid id,
        UpdatePermissionRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Permission ID is required.",
                nameof(id));
        }

        // --------------------------------------------------------
        // Validate Request
        // --------------------------------------------------------

        ArgumentException.ThrowIfNullOrWhiteSpace(
            request.Name,
            nameof(request.Name));

        ArgumentException.ThrowIfNullOrWhiteSpace(
            request.Code,
            nameof(request.Code));

        ArgumentException.ThrowIfNullOrWhiteSpace(
            request.Module,
            nameof(request.Module));

        var permission =
            await _unitOfWork.Permissions.GetByIdAsync(
                id,
                cancellationToken);

        if (permission is null)
        {
            throw new KeyNotFoundException(
                $"Permission with ID '{id}' was not found.");
        }

        // --------------------------------------------------------
        // Duplicate Name
        // --------------------------------------------------------

        var existingByName =
            await _unitOfWork.Permissions.GetByNameAsync(
                request.Name.Trim(),
                cancellationToken);

        if (existingByName is not null &&
            existingByName.Id != id)
        {
            throw new InvalidOperationException(
                $"A permission with the name '{request.Name.Trim()}' already exists.");
        }

        // --------------------------------------------------------
        // Duplicate Code
        // --------------------------------------------------------

        var existingByCode =
            await _unitOfWork.Permissions.ExistsAsync(
                permissionEntity =>
                    permissionEntity.Code == request.Code.Trim()
                    &&
                    permissionEntity.Id != id,
                cancellationToken);

        if (existingByCode)
        {
            throw new InvalidOperationException(
                $"A permission with the code '{request.Code.Trim()}' already exists.");
        }

        // --------------------------------------------------------
        // Update Domain Entity
        // --------------------------------------------------------

        permission.Update(
            request.Name,
            request.Code,
            request.Description,
            request.Module,
            request.UpdatedBy);

        _unitOfWork.Permissions.Update(permission);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(permission);
    }


    // ============================================================
    // Activate
    // ============================================================

    public async Task<PermissionResponseDto> ActivateAsync(
        Guid id,
        Guid updatedBy,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Permission ID is required.",
                nameof(id));
        }

        if (updatedBy == Guid.Empty)
        {
            throw new ArgumentException(
                "Updated by user ID is required.",
                nameof(updatedBy));
        }

        var permission =
            await _unitOfWork.Permissions.GetByIdAsync(
                id,
                cancellationToken);

        if (permission is null)
        {
            throw new KeyNotFoundException(
                $"Permission with ID '{id}' was not found.");
        }

        permission.Activate(updatedBy);

        _unitOfWork.Permissions.Update(permission);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(permission);
    }


    // ============================================================
    // Deactivate
    // ============================================================

    public async Task<PermissionResponseDto> DeactivateAsync(
        Guid id,
        Guid updatedBy,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Permission ID is required.",
                nameof(id));
        }

        if (updatedBy == Guid.Empty)
        {
            throw new ArgumentException(
                "Updated by user ID is required.",
                nameof(updatedBy));
        }

        var permission =
            await _unitOfWork.Permissions.GetByIdAsync(
                id,
                cancellationToken);

        if (permission is null)
        {
            throw new KeyNotFoundException(
                $"Permission with ID '{id}' was not found.");
        }

        permission.Deactivate(updatedBy);

        _unitOfWork.Permissions.Update(permission);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(permission);
    }


    // ============================================================
    // Soft Delete
    // ============================================================

    public async Task DeleteAsync(
        Guid id,
        Guid deletedBy,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Permission ID is required.",
                nameof(id));
        }

        if (deletedBy == Guid.Empty)
        {
            throw new ArgumentException(
                "Deleted by user ID is required.",
                nameof(deletedBy));
        }

        var permission =
            await _unitOfWork.Permissions.GetByIdAsync(
                id,
                cancellationToken);

        if (permission is null)
        {
            throw new KeyNotFoundException(
                $"Permission with ID '{id}' was not found.");
        }

        permission.MarkDeleted(deletedBy);

        _unitOfWork.Permissions.Update(permission);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }


    // ============================================================
    // Response Mapping
    // ============================================================

    private static PermissionResponseDto MapToResponse(
        Permission permission)
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


    // ============================================================
    // Detail Mapping
    // ============================================================

    private static PermissionDetailResponseDto
        MapToDetailResponse(
            Permission permission)
    {
        var activeRoleCount =
            permission.RolePermissions
                .Count(x => x.IsActive);

        return new PermissionDetailResponseDto
        {
            Id = permission.Id,

            Name = permission.Name,

            Code = permission.Code,

            Description = permission.Description,

            Module = permission.Module,

            IsActive = permission.IsActive,

            CreatedAt = permission.CreatedAt,

            CreatedBy = permission.CreatedBy,

            UpdatedAt = permission.UpdatedAt,

            UpdatedBy = permission.UpdatedBy,

            ActiveRoleCount = activeRoleCount
        };
    }
}

