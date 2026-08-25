using UAMS.Application.DTOs.AssetCategories.Requests;
using UAMS.Application.DTOs.AssetCategories.Responses;
using UAMS.Application.Interfaces.Persistence;
using UAMS.Application.Interfaces.Services;
using UAMS.Domain.Entities.AssetCategories;

namespace UAMS.Application.Services;

public class AssetCategoryService : IAssetCategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public AssetCategoryService(
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork
            ?? throw new ArgumentNullException(nameof(unitOfWork));
    }


    // ================================================================
    // Get By Id
    // ================================================================

    public async Task<AssetCategoryResponseDto> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Asset category ID is required.",
                nameof(id));
        }

        var category = await _unitOfWork.AssetCategories
            .GetByIdAsync(
                id,
                cancellationToken);

        if (category is null || category.IsDeleted)
        {
            throw new KeyNotFoundException(
                $"Asset category with ID '{id}' was not found.");
        }

        return MapToResponse(category);
    }


    // ================================================================
    // Get Details
    // ================================================================

    public async Task<AssetCategoryDetailResponseDto> GetDetailsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Asset category ID is required.",
                nameof(id));
        }

        var category = await _unitOfWork.AssetCategories
            .GetByIdWithDetailsAsync(
                id, 
                cancellationToken
            );

        if (category is null || category.IsDeleted)
        {
            throw new KeyNotFoundException(
                $"Asset category with ID '{id}' was not found.");
        }

        return new AssetCategoryDetailResponseDto
        {
            Id = category.Id,
            Code = category.Code,
            Name = category.Name,
            Description = category.Description,
            IsActive = category.IsActive,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt,
            AssetCount = category.Assets.Count
        };
    }


    // ================================================================
    // Get All / Filter / Pagination
    // ================================================================

    public async Task<AssetCategoryListResponseDto> GetAllAsync(
        AssetCategoryFilterRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var categories = request.IsActive switch
        {
            true => await _unitOfWork.AssetCategories
                .GetActiveCategoriesAsync(cancellationToken),

            false => await _unitOfWork.AssetCategories
                .GetInactiveCategoriesAsync(cancellationToken),

            null => await _unitOfWork.AssetCategories
                .GetAllAsync(cancellationToken)
        };

        var query = categories
            .Where(category => !category.IsDeleted)
            .AsEnumerable();


        // ============================================================
        // Search
        // ============================================================

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();

            query = query.Where(category =>
                category.Name.Contains(
                    search,
                    StringComparison.OrdinalIgnoreCase)
                ||
                category.Code.Contains(
                    search,
                    StringComparison.OrdinalIgnoreCase)
                ||
                (
                    category.Description != null
                    &&
                    category.Description.Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase)
                ));
        }


        // ============================================================
        // Ordering
        // ============================================================

        query = query
            .OrderBy(category => category.Name);


        // ============================================================
        // Pagination
        // ============================================================

        var totalCount = query.Count();

        var pageNumber = request.PageNumber < 1
            ? 1
            : request.PageNumber;

        var pageSize = request.PageSize < 1
            ? 20
            : Math.Min(request.PageSize, 100);

        var totalPages = totalCount == 0
            ? 0
            : (int)Math.Ceiling(
                totalCount / (double)pageSize);

        var items = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(MapToResponse)
            .ToList();


        return new AssetCategoryListResponseDto
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }


    // ================================================================
    // Create
    // ================================================================

    public async Task<AssetCategoryResponseDto> CreateAsync(
        CreateAssetCategoryRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var code = request.Code.Trim();
        var name = request.Name.Trim();


        // ============================================================
        // Duplicate Code
        // ============================================================

        var existingCode = await _unitOfWork.AssetCategories
            .FindAsync(
                category =>
                    category.Code == code
                    &&
                    !category.IsDeleted,
                cancellationToken);

        if (existingCode.Count > 0)
        {
            throw new InvalidOperationException(
                $"Asset category code '{code}' already exists.");
        }


        // ============================================================
        // Duplicate Name
        // ============================================================

        var existingName = await _unitOfWork.AssetCategories
            .GetByNameAsync(
                name,
                cancellationToken);

        if (existingName is not null
            && !existingName.IsDeleted)
        {
            throw new InvalidOperationException(
                $"Asset category name '{name}' already exists.");
        }


        // ============================================================
        // Create Domain Entity
        // ============================================================

        var category = AssetCategory.Create(
            code,
            name,
            request.Description);


        await _unitOfWork.AssetCategories.AddAsync(
            category,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(category);
    }


    // ================================================================
    // Update
    // ================================================================

    public async Task<AssetCategoryResponseDto> UpdateAsync(
        Guid id,
        UpdateAssetCategoryRequestDto request,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Asset category ID is required.",
                nameof(id));
        }

        ArgumentNullException.ThrowIfNull(request);


        var category = await _unitOfWork.AssetCategories
            .GetByIdAsync(
                id,
                cancellationToken);

        if (category is null || category.IsDeleted)
        {
            throw new KeyNotFoundException(
                $"Asset category with ID '{id}' was not found.");
        }


        var code = request.Code.Trim();
        var name = request.Name.Trim();


        // ============================================================
        // Duplicate Code
        // ============================================================

        var duplicateCode = await _unitOfWork.AssetCategories
            .FindAsync(
                existing =>
                    existing.Id != id
                    &&
                    existing.Code == code
                    &&
                    !existing.IsDeleted,
                cancellationToken);

        if (duplicateCode.Count > 0)
        {
            throw new InvalidOperationException(
                $"Asset category code '{code}' already exists.");
        }


        // ============================================================
        // Duplicate Name
        // ============================================================

        var existingName = await _unitOfWork.AssetCategories
            .GetByNameAsync(
                name,
                cancellationToken);

        if (existingName is not null
            && existingName.Id != id
            && !existingName.IsDeleted)
        {
            throw new InvalidOperationException(
                $"Asset category name '{name}' already exists.");
        }


        // ============================================================
        // Update Domain Entity
        // ============================================================

        category.Update(
            code,
            name,
            request.Description);

        _unitOfWork.AssetCategories.Update(category);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(category);
    }


    // ================================================================
    // Activate
    // ================================================================

    public async Task<AssetCategoryResponseDto> ActivateAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Asset category ID is required.",
                nameof(id));
        }

        var category = await _unitOfWork.AssetCategories
            .GetByIdAsync(
                id,
                cancellationToken);

        if (category is null || category.IsDeleted)
        {
            throw new KeyNotFoundException(
                $"Asset category with ID '{id}' was not found.");
        }

        category.Activate();

        _unitOfWork.AssetCategories.Update(category);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(category);
    }


    // ================================================================
    // Deactivate
    // ================================================================

    public async Task<AssetCategoryResponseDto> DeactivateAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Asset category ID is required.",
                nameof(id));
        }

        var category = await _unitOfWork.AssetCategories
            .GetByIdAsync(
                id,
                cancellationToken);

        if (category is null || category.IsDeleted)
        {
            throw new KeyNotFoundException(
                $"Asset category with ID '{id}' was not found.");
        }

        category.Deactivate();

        _unitOfWork.AssetCategories.Update(category);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(category);
    }


    // ================================================================
    // Mapping
    // ================================================================

    private static AssetCategoryResponseDto MapToResponse(
        AssetCategory category)
    {
        return new AssetCategoryResponseDto
        {
            Id = category.Id,
            Code = category.Code,
            Name = category.Name,
            Description = category.Description,
            IsActive = category.IsActive,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt
        };
    }
}

