using UAMS.Application.DTOs.QRCode.Requests;
using UAMS.Application.DTOs.QRCode.Responses;
using UAMS.Application.Interfaces.Persistence;
using UAMS.Application.Interfaces.Services;
using UAMS.Domain.Entities.QRCodes;

namespace UAMS.Application.Services;

public class QRCodeService : IQRCodeService
{
    private readonly IUnitOfWork _unitOfWork;

    public QRCodeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork
            ?? throw new ArgumentNullException(nameof(unitOfWork));
    }


    // ================================================================
    // Get QR Code By ID
    // ================================================================

    public async Task<QRCodeResponseDto> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        ValidateId(id);

        var qrCode = await _unitOfWork.QRCodes
            .GetByIdAsync(
                id,
                cancellationToken);

        if (qrCode is null)
        {
            throw new KeyNotFoundException(
                $"QR code with ID '{id}' was not found.");
        }

        return MapToResponse(qrCode);
    }


    // ================================================================
    // Get QR Code Details
    // ================================================================

    public async Task<QRCodeDetailResponseDto> GetDetailsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        ValidateId(id);

        var qrCode = await _unitOfWork.QRCodes
            .GetByIdWithDetailsAsync(
                id,
                cancellationToken);

        if (qrCode is null)
        {
            throw new KeyNotFoundException(
                $"QR code with ID '{id}' was not found.");
        }

        return MapToDetailResponse(qrCode);
    }


    // ================================================================
    // Get By Code
    // ================================================================

    public async Task<QRCodeResponseDto> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);

        var qrCode = await _unitOfWork.QRCodes
            .GetByCodeAsync(
                code.Trim(),
                cancellationToken);

        if (qrCode is null)
        {
            throw new KeyNotFoundException(
                $"QR code '{code}' was not found.");
        }

        return MapToResponse(qrCode);
    }


    // ================================================================
    // Get By Asset
    // ================================================================

    public async Task<QRCodeResponseDto> GetByAssetIdAsync(
        Guid assetId,
        CancellationToken cancellationToken = default)
    {
        ValidateAssetId(assetId);

        var qrCode = await _unitOfWork.QRCodes
            .GetByAssetIdAsync(
                assetId,
                cancellationToken);

        if (qrCode is null)
        {
            throw new KeyNotFoundException(
                $"QR code for asset '{assetId}' was not found.");
        }

        return MapToResponse(qrCode);
    }


    // ================================================================
    // Get Active By Asset
    // ================================================================

    public async Task<QRCodeResponseDto> GetActiveByAssetIdAsync(
        Guid assetId,
        CancellationToken cancellationToken = default)
    {
        ValidateAssetId(assetId);

        var qrCode = await _unitOfWork.QRCodes
            .GetActiveByAssetIdAsync(
                assetId,
                cancellationToken);

        if (qrCode is null)
        {
            throw new KeyNotFoundException(
                $"No active QR code exists for asset '{assetId}'.");
        }

        return MapToResponse(qrCode);
    }


    // ================================================================
    // Get All QR Codes
    // ================================================================

    public async Task<QRCodeListResponseDto> GetAllAsync(
        QRCodeFilterRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var qrCodes = await _unitOfWork.QRCodes
            .GetAllAsync(cancellationToken);

        IEnumerable<QRCode> query = qrCodes;

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

        return new QRCodeListResponseDto
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
    // Generate QR Code
    // ================================================================

    public async Task<QRCodeResponseDto> GenerateAsync(
        GenerateQRCodeRequestDto request,
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
                "Cannot generate a QR code for an inactive asset.");
        }

        var activeQRCode = await _unitOfWork.QRCodes
            .GetActiveByAssetIdAsync(
                request.AssetId,
                cancellationToken);

        if (activeQRCode is not null)
        {
            throw new InvalidOperationException(
                "An active QR code already exists for this asset.");
        }

        var generatedAt = DateTime.UtcNow;

        var code = GenerateCode();

        var encodedData = BuildEncodedData(
            asset.Id,
            asset.AssetTag,
            code);

        var qrCode = QRCode.Create(
            asset.Id,
            code,
            encodedData,
            null,
            generatedAt,
            request.ExpiresAt);

        await _unitOfWork.QRCodes.AddAsync(
            qrCode,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(qrCode);
    }


    // ================================================================
    // Update QR Code
    // ================================================================

    public async Task<QRCodeResponseDto> UpdateAsync(
        Guid id,
        UpdateQRCodeRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ValidateId(id);

        ArgumentNullException.ThrowIfNull(request);

        var qrCode = await _unitOfWork.QRCodes
            .GetByIdAsync(
                id,
                cancellationToken);

        if (qrCode is null)
        {
            throw new KeyNotFoundException(
                $"QR code with ID '{id}' was not found.");
        }

        qrCode.UpdateExpiration(
            request.ExpiresAt);

        _unitOfWork.QRCodes.Update(qrCode);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(qrCode);
    }


    // ================================================================
    // Delete QR Code
    // ================================================================

    public async Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        ValidateId(id);

        var qrCode = await _unitOfWork.QRCodes
            .GetByIdAsync(
                id,
                cancellationToken);

        if (qrCode is null)
        {
            throw new KeyNotFoundException(
                $"QR code with ID '{id}' was not found.");
        }

        _unitOfWork.QRCodes.Delete(qrCode);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }


    // ================================================================
    // Filtering
    // ================================================================

    private static IEnumerable<QRCode> ApplyFilters(
        IEnumerable<QRCode> query,
        QRCodeFilterRequestDto request)
    {
        if (request.AssetId.HasValue)
        {
            query = query.Where(qrCode =>
                qrCode.AssetId ==
                request.AssetId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Code))
        {
            var code = request.Code.Trim();

            query = query.Where(qrCode =>
                qrCode.Code.Contains(
                    code,
                    StringComparison.OrdinalIgnoreCase));
        }

        if (request.IsActive.HasValue)
        {
            var now = DateTime.UtcNow;

            query = request.IsActive.Value
                ? query.Where(qrCode =>
                    qrCode.ExpiresAt == null ||
                    qrCode.ExpiresAt > now)
                : query.Where(qrCode =>
                    qrCode.ExpiresAt.HasValue &&
                    qrCode.ExpiresAt <= now);
        }

        if (request.GeneratedFrom.HasValue)
        {
            query = query.Where(qrCode =>
                qrCode.GeneratedAt >=
                request.GeneratedFrom.Value);
        }

        if (request.GeneratedTo.HasValue)
        {
            var endDateExclusive =
                request.GeneratedTo.Value.Date.AddDays(1);

            query = query.Where(qrCode =>
                qrCode.GeneratedAt <
                endDateExclusive);
        }

        if (request.ExpiresFrom.HasValue)
        {
            query = query.Where(qrCode =>
                qrCode.ExpiresAt.HasValue &&
                qrCode.ExpiresAt.Value >=
                request.ExpiresFrom.Value);
        }

        if (request.ExpiresTo.HasValue)
        {
            var endDateExclusive =
                request.ExpiresTo.Value.Date.AddDays(1);

            query = query.Where(qrCode =>
                qrCode.ExpiresAt.HasValue &&
                qrCode.ExpiresAt.Value <
                endDateExclusive);
        }

        if (request.IsExpired.HasValue)
        {
            var now = DateTime.UtcNow;

            query = request.IsExpired.Value
                ? query.Where(qrCode =>
                    qrCode.ExpiresAt.HasValue &&
                    qrCode.ExpiresAt.Value <= now)
                : query.Where(qrCode =>
                    qrCode.ExpiresAt == null ||
                    qrCode.ExpiresAt > now);
        }

        return query;
    }


    // ================================================================
    // Ordering
    // ================================================================

    private static IEnumerable<QRCode> ApplyOrdering(
        IEnumerable<QRCode> query,
        QRCodeFilterRequestDto request)
    {
        return request.SortBy?.Trim().ToLowerInvariant() switch
        {
            "code" => request.SortDescending
                ? query.OrderByDescending(qrCode => qrCode.Code)
                : query.OrderBy(qrCode => qrCode.Code),

            "generatedat" => request.SortDescending
                ? query.OrderByDescending(qrCode => qrCode.GeneratedAt)
                : query.OrderBy(qrCode => qrCode.GeneratedAt),

            "expiresat" => request.SortDescending
                ? query.OrderByDescending(qrCode => qrCode.ExpiresAt)
                : query.OrderBy(qrCode => qrCode.ExpiresAt),

            _ => query.OrderByDescending(
                qrCode => qrCode.GeneratedAt)
        };
    }


    // ================================================================
    // Code Generation
    // ================================================================

    private static string GenerateCode()
    {
        return $"QR-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid():N}";
    }


    // ================================================================
    // Encoded Data
    // ================================================================

    private static string BuildEncodedData(
        Guid assetId,
        string assetTag,
        string code)
    {
        return
            $"UAMS|AssetId={assetId}|AssetTag={assetTag}|QRCode={code}";
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

    private static QRCodeResponseDto MapToResponse(
        QRCode qrCode)
    {
        var now = DateTime.UtcNow;

        return new QRCodeResponseDto
        {
            Id = qrCode.Id,
            AssetId = qrCode.AssetId,
            Code = qrCode.Code,
            EncodedData = qrCode.EncodedData,
            ImagePath = qrCode.ImagePath,
            GeneratedAt = qrCode.GeneratedAt,
            ExpiresAt = qrCode.ExpiresAt,
            IsActive =
                qrCode.ExpiresAt == null ||
                qrCode.ExpiresAt > now,
            IsExpired =
                qrCode.ExpiresAt.HasValue &&
                qrCode.ExpiresAt.Value <= now,
            CreatedAt = qrCode.CreatedAt,
            UpdatedAt = qrCode.UpdatedAt
        };
    }


    private static QRCodeDetailResponseDto
        MapToDetailResponse(QRCode qrCode)
    {
        var now = DateTime.UtcNow;

        return new QRCodeDetailResponseDto
        {
            Id = qrCode.Id,

            Code = qrCode.Code,

            EncodedData =
                qrCode.EncodedData,

            ImagePath =
                qrCode.ImagePath,

            GeneratedAt =
                qrCode.GeneratedAt,

            ExpiresAt =
                qrCode.ExpiresAt,

            IsActive =
                qrCode.ExpiresAt == null ||
                qrCode.ExpiresAt > now,

            IsExpired =
                qrCode.ExpiresAt.HasValue &&
                qrCode.ExpiresAt.Value <= now,

            AssetId =
                qrCode.AssetId,

            AssetTag =
                qrCode.Asset?.AssetTag,

            AssetName =
                qrCode.Asset?.Name,

            SerialNumber =
                qrCode.Asset?.SerialNumber,

            AssetStatus =
                qrCode.Asset?.Status.ToString(),

            CreatedAt =
                qrCode.CreatedAt,

            UpdatedAt =
                qrCode.UpdatedAt
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
                "QR code ID is required.",
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