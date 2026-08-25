using UAMS.Application.DTOs.Assets.Requests;
using UAMS.Application.DTOs.Assets.Responses;
using UAMS.Application.Interfaces.Persistence;
using UAMS.Application.Interfaces.Services;
using UAMS.Domain.Entities.Assets;

namespace UAMS.Application.Services;

public class AssetService : IAssetService
{
    private readonly IUnitOfWork _unitOfWork;

    public AssetService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork
            ?? throw new ArgumentNullException(nameof(unitOfWork));
    }


    // ================================================================
    // Get Asset By ID
    // ================================================================

    public async Task<AssetResponseDto> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        ValidateId(id);

        var asset = await _unitOfWork.Assets
            .GetByIdAsync(
                id,
                cancellationToken);

        if (asset is null)
        {
            throw new KeyNotFoundException(
                $"Asset with ID '{id}' was not found.");
        }

        return MapToResponse(asset);
    }


// ================================================================
// Get Asset Details
// ================================================================

public async Task<AssetDetailResponseDto> GetDetailsAsync(
    Guid id,
    CancellationToken cancellationToken = default)
{
    ValidateId(id);

    var asset = await _unitOfWork.Assets
        .GetByIdWithDetailsAsync(
            id,
            cancellationToken);

    if (asset is null)
    {
        throw new KeyNotFoundException(
            $"Asset with ID '{id}' was not found.");
    }

    return MapToDetailResponse(asset);
}


    // ================================================================
    // Get Assets
    // ================================================================

    public async Task<AssetListResponseDto> GetAllAsync(
        AssetFilterRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var assets = await _unitOfWork.Assets
            .GetAllAsync(cancellationToken);

        var query = assets.AsEnumerable();

        query = ApplyFilters(query, request);

        query = ApplyOrdering(query, request);

        return CreatePagedResponse(
            query,
            request);
    }


    // ================================================================
    // Create Asset
    // ================================================================

    public async Task<AssetResponseDto> CreateAsync(
        CreateAssetRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        await ValidateCreateRequestAsync(
            request,
            cancellationToken);

        var asset = Asset.Create(
            request.AssetTag.Trim(),
            request.Name.Trim(),
            request.Description,
            request.SerialNumber,
            request.Model,
            request.Manufacturer,
            request.AssetCategoryId,
            request.PurchaseId,
            request.DepartmentId,
            request.PurchaseCost,
            request.PurchaseDate,
            request.WarrantyExpiryDate,
            request.Location,
            request.Status,
            request.Condition);

        await _unitOfWork.Assets.AddAsync(
            asset,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(asset);
    }


    // ================================================================
    // Update Asset
    // ================================================================

    public async Task<AssetResponseDto> UpdateAsync(
        Guid id,
        UpdateAssetRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ValidateId(id);

        ArgumentNullException.ThrowIfNull(request);

        var asset = await _unitOfWork.Assets
            .GetByIdAsync(
                id,
                cancellationToken);

        if (asset is null)
        {
            throw new KeyNotFoundException(
                $"Asset with ID '{id}' was not found.");
        }

        await ValidateUpdateRequestAsync(
            request,
            cancellationToken);

        asset.Update(
            request.Name.Trim(),
            request.Description,
            request.SerialNumber,
            request.Model,
            request.Manufacturer,
            request.AssetCategoryId,
            request.DepartmentId,
            request.PurchaseCost,
            request.PurchaseDate,
            request.WarrantyExpiryDate,
            request.Location);

        _unitOfWork.Assets.Update(asset);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(asset);
    }


// ================================================================
// Delete Asset
// ================================================================

public async Task DeleteAsync(
    Guid id,
    CancellationToken cancellationToken = default)
{
    ValidateId(id);

    var asset = await _unitOfWork.Assets
        .GetByIdWithDetailsAsync(
            id,
            cancellationToken);

    if (asset is null)
    {
        throw new KeyNotFoundException(
            $"Asset with ID '{id}' was not found.");
    }

    ValidateDeletion(asset);

    _unitOfWork.Assets.Delete(asset);

    await _unitOfWork.SaveChangesAsync(
        cancellationToken);
}

    // ================================================================
    // Create Validation
    // ================================================================

    private async Task ValidateCreateRequestAsync(
        CreateAssetRequestDto request,
        CancellationToken cancellationToken)
    {
        await ValidateAssetTagAsync(
            request.AssetTag,
            cancellationToken);

        await ValidateSerialNumberAsync(
            request.SerialNumber,
            cancellationToken);

        await ValidateCategoryAsync(
            request.AssetCategoryId,
            cancellationToken);

        await ValidatePurchaseAsync(
            request.PurchaseId,
            cancellationToken);

        await ValidateDepartmentAsync(
            request.DepartmentId,
            cancellationToken);
    }


    // ================================================================
    // Update Validation
    // ================================================================

    private async Task ValidateUpdateRequestAsync(
        UpdateAssetRequestDto request,
        CancellationToken cancellationToken)
    {
        await ValidateCategoryAsync(
            request.AssetCategoryId,
            cancellationToken);

        await ValidateDepartmentAsync(
            request.DepartmentId,
            cancellationToken);
    }


    // ================================================================
    // Validate Asset Tag
    // ================================================================

    private async Task ValidateAssetTagAsync(
        string assetTag,
        CancellationToken cancellationToken)
    {
        var existingAsset = await _unitOfWork.Assets
            .GetByAssetNumberAsync(
                assetTag.Trim(),
                cancellationToken);

        if (existingAsset is not null)
        {
            throw new InvalidOperationException(
                $"An asset with asset tag '{assetTag}' already exists.");
        }
    }


    // ================================================================
    // Validate Serial Number
    // ================================================================

    private async Task ValidateSerialNumberAsync(
        string? serialNumber,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(serialNumber))
        {
            return;
        }

        var existingAsset = await _unitOfWork.Assets
            .GetBySerialNumberAsync(
                serialNumber.Trim(),
                cancellationToken);

        if (existingAsset is not null)
        {
            throw new InvalidOperationException(
                $"An asset with serial number '{serialNumber}' already exists.");
        }
    }


    // ================================================================
    // Validate Category
    // ================================================================

    private async Task ValidateCategoryAsync(
        Guid assetCategoryId,
        CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.AssetCategories
            .GetByIdAsync(
                assetCategoryId,
                cancellationToken);

        if (category is null)
        {
            throw new KeyNotFoundException(
                $"Asset category with ID '{assetCategoryId}' was not found.");
        }

        if (!category.IsActive)
        {
            throw new InvalidOperationException(
                "Cannot assign an inactive asset category to an asset.");
        }
    }


    // ================================================================
    // Validate Purchase
    // ================================================================

    private async Task ValidatePurchaseAsync(
        Guid purchaseId,
        CancellationToken cancellationToken)
    {
        var purchase = await _unitOfWork.Purchases
            .GetByIdAsync(
                purchaseId,
                cancellationToken);

        if (purchase is null)
        {
            throw new KeyNotFoundException(
                $"Purchase with ID '{purchaseId}' was not found.");
        }

        if (!purchase.IsActive)
        {
            throw new InvalidOperationException(
                "Cannot register an asset against an inactive purchase.");
        }
    }


    // ================================================================
    // Validate Department
    // ================================================================

    private async Task ValidateDepartmentAsync(
        Guid? departmentId,
        CancellationToken cancellationToken)
    {
        if (!departmentId.HasValue)
        {
            return;
        }

        var department = await _unitOfWork.Departments
            .GetByIdAsync(
                departmentId.Value,
                cancellationToken);

        if (department is null)
        {
            throw new KeyNotFoundException(
                $"Department with ID '{departmentId.Value}' was not found.");
        }

        if (!department.IsActive)
        {
            throw new InvalidOperationException(
                "Cannot assign an inactive department to an asset.");
        }
    }


    // ================================================================
    // Apply Filters
    // ================================================================

    private static IEnumerable<Asset> ApplyFilters(
        IEnumerable<Asset> query,
        AssetFilterRequestDto request)
    {
        if (!string.IsNullOrWhiteSpace(request.AssetTag))
        {
            var value = request.AssetTag.Trim();

            query = query.Where(asset =>
                asset.AssetTag.Contains(
                    value,
                    StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var value = request.Name.Trim();

            query = query.Where(asset =>
                asset.Name.Contains(
                    value,
                    StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(request.SerialNumber))
        {
            var value = request.SerialNumber.Trim();

            query = query.Where(asset =>
                asset.SerialNumber != null &&
                asset.SerialNumber.Contains(
                    value,
                    StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(request.Manufacturer))
        {
            var value = request.Manufacturer.Trim();

            query = query.Where(asset =>
                asset.Manufacturer != null &&
                asset.Manufacturer.Contains(
                    value,
                    StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(request.Model))
        {
            var value = request.Model.Trim();

            query = query.Where(asset =>
                asset.Model != null &&
                asset.Model.Contains(
                    value,
                    StringComparison.OrdinalIgnoreCase));
        }

        if (request.AssetCategoryId.HasValue)
        {
            query = query.Where(asset =>
                asset.AssetCategoryId ==
                request.AssetCategoryId.Value);
        }

        if (request.PurchaseId.HasValue)
        {
            query = query.Where(asset =>
                asset.PurchaseId ==
                request.PurchaseId.Value);
        }

        if (request.DepartmentId.HasValue)
        {
            query = query.Where(asset =>
                asset.DepartmentId ==
                request.DepartmentId.Value);
        }

        if (request.Status.HasValue)
        {
            query = query.Where(asset =>
                asset.Status ==
                request.Status.Value);
        }

        if (request.Condition.HasValue)
        {
            query = query.Where(asset =>
                asset.Condition ==
                request.Condition.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Location))
        {
            var value = request.Location.Trim();

            query = query.Where(asset =>
                asset.Location != null &&
                asset.Location.Contains(
                    value,
                    StringComparison.OrdinalIgnoreCase));
        }

        if (request.PurchaseDateFrom.HasValue)
        {
            query = query.Where(asset =>
                asset.PurchaseDate >=
                request.PurchaseDateFrom.Value);
        }

        if (request.PurchaseDateTo.HasValue)
        {
            var endDateExclusive =
                request.PurchaseDateTo.Value.Date.AddDays(1);

            query = query.Where(asset =>
                asset.PurchaseDate < endDateExclusive);
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(asset =>
                asset.IsActive ==
                request.IsActive.Value);
        }

        return query;
    }


    // ================================================================
    // Apply Ordering
    // ================================================================

    private static IEnumerable<Asset> ApplyOrdering(
        IEnumerable<Asset> query,
        AssetFilterRequestDto request)
    {
        var sortBy = request.SortBy?.Trim();

        if (string.IsNullOrWhiteSpace(sortBy))
        {
            return query.OrderBy(asset => asset.AssetTag);
        }

        return sortBy.ToLowerInvariant() switch
        {
            "assettag" => request.SortDescending
                ? query.OrderByDescending(asset => asset.AssetTag)
                : query.OrderBy(asset => asset.AssetTag),

            "name" => request.SortDescending
                ? query.OrderByDescending(asset => asset.Name)
                : query.OrderBy(asset => asset.Name),

            "purchasedate" => request.SortDescending
                ? query.OrderByDescending(asset => asset.PurchaseDate)
                : query.OrderBy(asset => asset.PurchaseDate),

            "purchasecost" => request.SortDescending
                ? query.OrderByDescending(asset => asset.PurchaseCost)
                : query.OrderBy(asset => asset.PurchaseCost),

            "status" => request.SortDescending
                ? query.OrderByDescending(asset => asset.Status)
                : query.OrderBy(asset => asset.Status),

            "condition" => request.SortDescending
                ? query.OrderByDescending(asset => asset.Condition)
                : query.OrderBy(asset => asset.Condition),

            _ => query.OrderBy(asset => asset.AssetTag)
        };
    }


    // ================================================================
    // Create Paged Response
    // ================================================================

    private static AssetListResponseDto CreatePagedResponse(
        IEnumerable<Asset> query,
        AssetFilterRequestDto request)
    {
        var totalCount = query.Count();

        var totalPages = totalCount == 0
            ? 0
            : (int)Math.Ceiling(
                totalCount /
                (double)request.PageSize);

        var items = query
            .Skip(
                (request.PageNumber - 1) *
                request.PageSize)
            .Take(request.PageSize)
            .Select(MapToResponse)
            .ToList();

        return new AssetListResponseDto
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            HasPreviousPage =
                request.PageNumber > 1,
            HasNextPage =
                request.PageNumber < totalPages
        };
    }

// ================================================================
// Validate Delete
// ================================================================

private static void ValidateDeletion(Asset asset)
{
    var deletionRules = new[]
    {
        new
        {
            HasRelatedData = asset.AssetAssignments.Any(),
            Message = "An asset cannot be deleted because it has associated assignments."
        },
        new
        {
            HasRelatedData = asset.AssetRequests.Any(),
            Message = "An asset cannot be deleted because it has associated requests."
        },
        new
        {
            HasRelatedData = asset.AssetTransfers.Any(),
            Message = "An asset cannot be deleted because it has associated transfers."
        },
        new
        {
            HasRelatedData = asset.AssetReturns.Any(),
            Message = "An asset cannot be deleted because it has associated returns."
        },
        new
        {
            HasRelatedData = asset.DamageReports.Any(),
            Message = "An asset cannot be deleted because it has associated damage reports."
        },
        new
        {
            HasRelatedData = asset.Maintenances.Any(),
            Message = "An asset cannot be deleted because it has associated maintenance records."
        },
        new
        {
            HasRelatedData = asset.AssetDisposals.Any(),
            Message = "An asset cannot be deleted because it has associated disposal records."
        }
    };

    var failedRule = deletionRules
        .FirstOrDefault(rule => rule.HasRelatedData);

    if (failedRule is not null)
    {
        throw new InvalidOperationException(failedRule.Message);
    }
}
    


    // ================================================================
    // Map To Response
    // ================================================================

    private static AssetResponseDto MapToResponse(
        Asset asset)
    {
        return new AssetResponseDto
        {
            Id = asset.Id,

            AssetTag =
                asset.AssetTag,

            Name =
                asset.Name,

            Description =
                asset.Description,

            SerialNumber =
                asset.SerialNumber,

            Model =
                asset.Model,

            Manufacturer =
                asset.Manufacturer,

            AssetCategoryId =
                asset.AssetCategoryId,

            AssetCategoryName =
                asset.AssetCategory?.Name,

            PurchaseId =
                asset.PurchaseId,

            PurchaseNumber =
                asset.Purchase?.PurchaseNumber,

            DepartmentId =
                asset.DepartmentId,

            DepartmentName =
                asset.Department?.Name,

            PurchaseCost =
                asset.PurchaseCost,

            PurchaseDate =
                asset.PurchaseDate,

            WarrantyExpiryDate =
                asset.WarrantyExpiryDate,

            Location =
                asset.Location,

            Status =
                asset.Status,

            Condition =
                asset.Condition,

            IsActive =
                asset.IsActive,

            CreatedAt =
                asset.CreatedAt,

            UpdatedAt =
                asset.UpdatedAt
        };
    }


    // ================================================================
    // Map To Detail Response
    // ================================================================

    private static AssetDetailResponseDto MapToDetailResponse(
        Asset asset)
    {
        return new AssetDetailResponseDto
        {
            Id = asset.Id,

            AssetTag =
                asset.AssetTag,

            Name =
                asset.Name,

            Description =
                asset.Description,

            SerialNumber =
                asset.SerialNumber,

            Model =
                asset.Model,

            Manufacturer =
                asset.Manufacturer,

            AssetCategoryId =
                asset.AssetCategoryId,

            AssetCategoryName =
                asset.AssetCategory?.Name,

            PurchaseId =
                asset.PurchaseId,

            PurchaseNumber =
                asset.Purchase?.PurchaseNumber,

            SupplierId =
                asset.Purchase?.SupplierId,

            SupplierName =
                asset.Purchase?.Supplier?.Name,

            PurchaseCost =
                asset.PurchaseCost,

            PurchaseDate =
                asset.PurchaseDate,

            InvoiceNumber =
                asset.Purchase?.InvoiceNumber,

            PurchaseOrderNumber =
                asset.Purchase?.PurchaseOrderNumber,

            DepartmentId =
                asset.DepartmentId,

            DepartmentName =
                asset.Department?.Name,

            WarrantyExpiryDate =
                asset.WarrantyExpiryDate,

            Location =
                asset.Location,

            Status =
                asset.Status,

            Condition =
                asset.Condition,

            HasQRCode =
                asset.QRCode is not null,

            HasBarcode =
                asset.Barcode is not null,

            HasActiveAssignment =
                asset.AssetAssignments.Any(),

            HasPendingRequest =
                asset.AssetRequests.Any(),

            HasPendingTransfer =
                asset.AssetTransfers.Any(),

            HasOpenDamageReport =
                asset.DamageReports.Any(),

            HasActiveMaintenance =
                asset.Maintenances.Any(),

            HasPendingDisposal =
                asset.AssetDisposals.Any(),

            IsActive =
                asset.IsActive,

            CreatedAt =
                asset.CreatedAt,

            UpdatedAt =
                asset.UpdatedAt
        };
    }


    // ================================================================
    // ID Validation
    // ================================================================

    private static void ValidateId(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Asset ID is required.",
                nameof(id));
        }
    }
}