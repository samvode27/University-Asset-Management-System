using UAMS.Application.DTOs.Barcode.Requests;
using UAMS.Application.DTOs.Barcode.Responses;
using UAMS.Application.Interfaces.Persistence;
using UAMS.Application.Interfaces.Services;
using UAMS.Domain.Entities.Barcodes;
using UAMS.Domain.Enums;

namespace UAMS.Application.Services;

public class BarcodeService : IBarcodeService
{
    private readonly IUnitOfWork _unitOfWork;

    public BarcodeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork
            ?? throw new ArgumentNullException(nameof(unitOfWork));
    }


    // ================================================================
    // Get Barcode By ID
    // ================================================================

    public async Task<BarcodeResponseDto> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        ValidateId(id);

        var barcode = await _unitOfWork.Barcodes
            .GetByIdAsync(
                id,
                cancellationToken);

        if (barcode is null)
        {
            throw new KeyNotFoundException(
                $"Barcode with ID '{id}' was not found.");
        }

        return MapToResponse(barcode);
    }


    // ================================================================
    // Get Barcode Details
    // ================================================================

    public async Task<BarcodeDetailResponseDto> GetDetailsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        ValidateId(id);

        var barcode = await _unitOfWork.Barcodes
            .GetByIdWithDetailsAsync(
                id,
                cancellationToken);

        if (barcode is null)
        {
            throw new KeyNotFoundException(
                $"Barcode with ID '{id}' was not found.");
        }

        return MapToDetailResponse(barcode);
    }


    // ================================================================
    // Get Barcode By Code
    // ================================================================

    public async Task<BarcodeResponseDto> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);

        var normalizedCode = code.Trim();

        var barcode = await _unitOfWork.Barcodes
            .GetByCodeAsync(
                normalizedCode,
                cancellationToken);

        if (barcode is null)
        {
            throw new KeyNotFoundException(
                $"Barcode '{normalizedCode}' was not found.");
        }

        return MapToResponse(barcode);
    }


    // ================================================================
    // Get Barcode By Asset
    // ================================================================

    public async Task<BarcodeResponseDto> GetByAssetIdAsync(
        Guid assetId,
        CancellationToken cancellationToken = default)
    {
        ValidateAssetId(assetId);

        var barcode = await _unitOfWork.Barcodes
            .GetByAssetIdAsync(
                assetId,
                cancellationToken);

        if (barcode is null)
        {
            throw new KeyNotFoundException(
                $"Barcode for asset '{assetId}' was not found.");
        }

        return MapToResponse(barcode);
    }


    // ================================================================
    // Get Active Barcode By Asset
    // ================================================================

    public async Task<BarcodeResponseDto> GetActiveByAssetIdAsync(
        Guid assetId,
        CancellationToken cancellationToken = default)
    {
        ValidateAssetId(assetId);

        var barcode = await _unitOfWork.Barcodes
            .GetActiveByAssetIdAsync(
                assetId,
                cancellationToken);

        if (barcode is null)
        {
            throw new KeyNotFoundException(
                $"No active barcode exists for asset '{assetId}'.");
        }

        return MapToResponse(barcode);
    }


    // ================================================================
    // Get All Barcodes
    // ================================================================

    public async Task<BarcodeListResponseDto> GetAllAsync(
        BarcodeFilterRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var barcodes = await _unitOfWork.Barcodes
            .GetAllAsync(cancellationToken);

        IEnumerable<Barcode> query = barcodes;

        query = ApplyFilters(query, request);

        query = ApplyOrdering(query, request);

        var totalCount = query.Count();

        var totalPages = CalculateTotalPages(
            totalCount,
            request.PageSize);

        var items = query
            .Skip(
                (request.PageNumber - 1) *
                request.PageSize)
            .Take(request.PageSize)
            .Select(MapToResponse)
            .ToList();

        return new BarcodeListResponseDto
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
    // Generate Barcode
    // ================================================================

    public async Task<BarcodeResponseDto> GenerateAsync(
        GenerateBarcodeRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        ValidateAssetId(request.AssetId);

        var asset = await _unitOfWork.Assets
            .GetByIdAsync(
                request.AssetId,
                cancellationToken);

        if (asset is null)
        {
            throw new KeyNotFoundException(
                $"Asset with ID '{request.AssetId}' was not found.");
        }

        if (!asset.IsActive)
        {
            throw new InvalidOperationException(
                "Cannot generate a barcode for an inactive asset.");
        }

        var activeBarcode = await _unitOfWork.Barcodes
            .GetActiveByAssetIdAsync(
                request.AssetId,
                cancellationToken);

        if (activeBarcode is not null)
        {
            throw new InvalidOperationException(
                "An active barcode already exists for this asset.");
        }

        var generatedAt = DateTime.UtcNow;

        var code = GenerateCode();

        var encodedData = BuildEncodedData(
            asset.Id,
            asset.AssetTag,
            code,
            request.Format);

        var barcode = Barcode.Create(
            asset.Id,
            code,
            encodedData,
            request.Format,
            null,
            generatedAt,
            request.ExpiresAt);

        await _unitOfWork.Barcodes.AddAsync(
            barcode,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(barcode);
    }


    // ================================================================
    // Update Barcode
    // ================================================================

    public async Task<BarcodeResponseDto> UpdateAsync(
        Guid id,
        UpdateBarcodeRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ValidateId(id);

        ArgumentNullException.ThrowIfNull(request);

        var barcode = await _unitOfWork.Barcodes
            .GetByIdAsync(
                id,
                cancellationToken);

        if (barcode is null)
        {
            throw new KeyNotFoundException(
                $"Barcode with ID '{id}' was not found.");
        }

        barcode.Update(
            request.Format,
            request.ExpiresAt);

        _unitOfWork.Barcodes.Update(barcode);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(barcode);
    }


    // ================================================================
    // Delete Barcode
    // ================================================================

    public async Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        ValidateId(id);

        var barcode = await _unitOfWork.Barcodes
            .GetByIdAsync(
                id,
                cancellationToken);

        if (barcode is null)
        {
            throw new KeyNotFoundException(
                $"Barcode with ID '{id}' was not found.");
        }

        _unitOfWork.Barcodes.Delete(barcode);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }


    // ================================================================
    // Filtering
    // ================================================================

    private static IEnumerable<Barcode> ApplyFilters(
        IEnumerable<Barcode> query,
        BarcodeFilterRequestDto request)
    {
        if (request.AssetId.HasValue)
        {
            query = query.Where(barcode =>
                barcode.AssetId ==
                request.AssetId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Code))
        {
            var code = request.Code.Trim();

            query = query.Where(barcode =>
                barcode.Code.Contains(
                    code,
                    StringComparison.OrdinalIgnoreCase));
        }

        if (request.Format.HasValue)
        {
            query = query.Where(barcode =>
                barcode.Format ==
                request.Format.Value);
        }

      if (request.IsActive.HasValue)


        if (request.GeneratedFrom.HasValue)
        {
            query = query.Where(barcode =>
                barcode.GeneratedAt >=
                request.GeneratedFrom.Value);
        }

        if (request.GeneratedTo.HasValue)
        {
            var endDateExclusive =
                request.GeneratedTo.Value.Date.AddDays(1);

            query = query.Where(barcode =>
                barcode.GeneratedAt <
                endDateExclusive);
        }

        if (request.ExpiresFrom.HasValue)
        {
            query = query.Where(barcode =>
                barcode.ExpiresAt.HasValue &&
                barcode.ExpiresAt.Value >=
                request.ExpiresFrom.Value);
        }

        if (request.ExpiresTo.HasValue)
        {
            var endDateExclusive =
                request.ExpiresTo.Value.Date.AddDays(1);

            query = query.Where(barcode =>
                barcode.ExpiresAt.HasValue &&
                barcode.ExpiresAt.Value <
                endDateExclusive);
        }

        if (request.IsExpired.HasValue)
        {
            var now = DateTime.UtcNow;

            query = request.IsExpired.Value
                ? query.Where(barcode =>
                    barcode.ExpiresAt.HasValue &&
                    barcode.ExpiresAt.Value <= now)
                : query.Where(barcode =>
                    barcode.ExpiresAt == null ||
                    barcode.ExpiresAt > now);
        }

        return query;
    }


    // ================================================================
    // Ordering
    // ================================================================

    private static IEnumerable<Barcode> ApplyOrdering(
        IEnumerable<Barcode> query,
        BarcodeFilterRequestDto request)
    {
        return request.SortBy?
            .Trim()
            .ToLowerInvariant() switch
        {
            "code" => request.SortDescending
                ? query.OrderByDescending(
                    barcode => barcode.Code)
                : query.OrderBy(
                    barcode => barcode.Code),

            "format" => request.SortDescending
                ? query.OrderByDescending(
                    barcode => barcode.Format)
                : query.OrderBy(
                    barcode => barcode.Format),

            "generatedat" => request.SortDescending
                ? query.OrderByDescending(
                    barcode => barcode.GeneratedAt)
                : query.OrderBy(
                    barcode => barcode.GeneratedAt),

            "expiresat" => request.SortDescending
                ? query.OrderByDescending(
                    barcode => barcode.ExpiresAt)
                : query.OrderBy(
                    barcode => barcode.ExpiresAt),

            _ => query.OrderByDescending(
                barcode => barcode.GeneratedAt)
        };
    }


    // ================================================================
    // Code Generation
    // ================================================================

    private static string GenerateCode()
    {
        return $"BC-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid():N}";
    }


    // ================================================================
    // Encoded Data
    // ================================================================

    private static string BuildEncodedData(
        Guid assetId,
        string assetTag,
        string code,
        BarcodeFormat format)
    {
        return
            $"UAMS|AssetId={assetId}|AssetTag={assetTag}|Barcode={code}|Format={format}";
    }


    // ================================================================
    // Pagination
    // ================================================================

    private static int CalculateTotalPages(
        int totalCount,
        int pageSize)
    {
        if (totalCount == 0)
        {
            return 0;
        }

        return (int)Math.Ceiling(
            totalCount / (double)pageSize);
    }


    // ================================================================
    // Mapping
    // ================================================================

    private static BarcodeResponseDto MapToResponse(
        Barcode barcode)
    {
        return new BarcodeResponseDto
        {
            Id = barcode.Id,
            AssetId = barcode.AssetId,
            Code = barcode.Code,
            EncodedData = barcode.EncodedData,
            Format = barcode.Format,
            ImagePath = barcode.ImagePath,
            GeneratedAt = barcode.GeneratedAt,
            ExpiresAt = barcode.ExpiresAt,
            IsActive = barcode.IsCurrentlyActive(),
            IsExpired = barcode.IsExpired(),
            CreatedAt = barcode.CreatedAt,
            UpdatedAt = barcode.UpdatedAt
        };
    }


    private static BarcodeDetailResponseDto MapToDetailResponse(
        Barcode barcode)
    {
        return new BarcodeDetailResponseDto
        {
            Id = barcode.Id,

            Code = barcode.Code,

            EncodedData = barcode.EncodedData,

            Format = barcode.Format,

            ImagePath = barcode.ImagePath,

            GeneratedAt = barcode.GeneratedAt,

            ExpiresAt = barcode.ExpiresAt,

            IsActive = barcode.IsCurrentlyActive(),

            IsExpired = barcode.IsExpired(),

            AssetId = barcode.AssetId,

            AssetTag = barcode.Asset?.AssetTag,

            AssetName = barcode.Asset?.Name,

            SerialNumber = barcode.Asset?.SerialNumber,

            AssetStatus = barcode.Asset?.Status.ToString(),

            CreatedAt = barcode.CreatedAt,

            UpdatedAt = barcode.UpdatedAt
        };
    }


    // ================================================================
    // Validation
    // ================================================================

    private static void ValidateId(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Barcode ID is required.",
                nameof(id));
        }
    }


    private static void ValidateAssetId(Guid assetId)
    {
        if (assetId == Guid.Empty)
        {
            throw new ArgumentException(
                "Asset ID is required.",
                nameof(assetId));
        }
    }
}