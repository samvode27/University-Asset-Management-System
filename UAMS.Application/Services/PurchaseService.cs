using UAMS.Application.DTOs.Purchases.Requests;
using UAMS.Application.DTOs.Purchases.Responses;
using UAMS.Application.Interfaces.Persistence;
using UAMS.Application.Interfaces.Services;
using UAMS.Domain.Entities.Purchases;

namespace UAMS.Application.Services;

public class PurchaseService : IPurchaseService
{
    private readonly IUnitOfWork _unitOfWork;

    public PurchaseService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork
            ?? throw new ArgumentNullException(nameof(unitOfWork));
    }


    // ================================================================
    // Get Purchase By ID
    // ================================================================

    public async Task<PurchaseResponseDto> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        ValidateId(id);

        var purchase = await _unitOfWork.Purchases
            .GetByIdAsync(
                id,
                cancellationToken);

        if (purchase is null)
        {
            throw new KeyNotFoundException(
                $"Purchase with ID '{id}' was not found.");
        }

        return MapToResponse(purchase);
    }


    // ================================================================
    // Get Purchase Details
    // ================================================================

    public async Task<PurchaseDetailResponseDto> GetDetailsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        ValidateId(id);

        var purchase = await _unitOfWork.Purchases
            .GetByIdWithDetailsAsync(
                id,
                cancellationToken);

        if (purchase is null)
        {
            throw new KeyNotFoundException(
                $"Purchase with ID '{id}' was not found.");
        }

        return MapToDetailResponse(purchase);
    }


    // ================================================================
    // Get Purchases
    // ================================================================

    public async Task<PurchaseListResponseDto> GetAllAsync(
        PurchaseFilterRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var purchases = await _unitOfWork.Purchases
            .GetAllAsync(cancellationToken);

        IEnumerable<Purchase> query = purchases;

        query = ApplySearchFilter(
            query,
            request);

        query = ApplySupplierFilter(
            query,
            request);

        query = ApplyPurchaseDateFilter(
            query,
            request);

        query = ApplyInvoiceFilter(
            query,
            request);

        query = ApplyPurchaseOrderFilter(
            query,
            request);

        query = ApplyCurrencyFilter(
            query,
            request);

        query = ApplyAmountFilter(
            query,
            request);

        query = ApplyStatusFilter(
            query,
            request);

        query = query
            .OrderByDescending(purchase =>
                purchase.PurchaseDate);

        return CreatePagedResponse(
            query,
            request);
    }


    // ================================================================
    // Purchase Filtering
    // ================================================================

    private static IEnumerable<Purchase> ApplySearchFilter(
        IEnumerable<Purchase> query,
        PurchaseFilterRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Search))
        {
            return query;
        }

        var search = request.Search.Trim();

        return query.Where(purchase =>
            purchase.PurchaseNumber.Contains(
                search,
                StringComparison.OrdinalIgnoreCase) ||

            (purchase.InvoiceNumber != null &&
             purchase.InvoiceNumber.Contains(
                 search,
                 StringComparison.OrdinalIgnoreCase)) ||

            (purchase.PurchaseOrderNumber != null &&
             purchase.PurchaseOrderNumber.Contains(
                 search,
                 StringComparison.OrdinalIgnoreCase)) ||

            (purchase.Description != null &&
             purchase.Description.Contains(
                 search,
                 StringComparison.OrdinalIgnoreCase)));
    }


    private static IEnumerable<Purchase> ApplySupplierFilter(
        IEnumerable<Purchase> query,
        PurchaseFilterRequestDto request)
    {
        if (!request.SupplierId.HasValue)
        {
            return query;
        }

        return query.Where(purchase =>
            purchase.SupplierId ==
            request.SupplierId.Value);
    }


    private static IEnumerable<Purchase> ApplyPurchaseDateFilter(
        IEnumerable<Purchase> query,
        PurchaseFilterRequestDto request)
    {
        if (request.PurchaseDateFrom.HasValue)
        {
            query = query.Where(purchase =>
                purchase.PurchaseDate >=
                request.PurchaseDateFrom.Value);
        }

        if (request.PurchaseDateTo.HasValue)
        {
            var endDateExclusive =
                request.PurchaseDateTo.Value
                    .Date
                    .AddDays(1);

            query = query.Where(purchase =>
                purchase.PurchaseDate <
                endDateExclusive);
        }

        return query;
    }


    private static IEnumerable<Purchase> ApplyInvoiceFilter(
        IEnumerable<Purchase> query,
        PurchaseFilterRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(
            request.InvoiceNumber))
        {
            return query;
        }

        var invoiceNumber =
            request.InvoiceNumber.Trim();

        return query.Where(purchase =>
            purchase.InvoiceNumber != null &&
            purchase.InvoiceNumber.Contains(
                invoiceNumber,
                StringComparison.OrdinalIgnoreCase));
    }


    private static IEnumerable<Purchase>
        ApplyPurchaseOrderFilter(
            IEnumerable<Purchase> query,
            PurchaseFilterRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(
            request.PurchaseOrderNumber))
        {
            return query;
        }

        var purchaseOrderNumber =
            request.PurchaseOrderNumber.Trim();

        return query.Where(purchase =>
            purchase.PurchaseOrderNumber != null &&
            purchase.PurchaseOrderNumber.Contains(
                purchaseOrderNumber,
                StringComparison.OrdinalIgnoreCase));
    }


    private static IEnumerable<Purchase> ApplyCurrencyFilter(
        IEnumerable<Purchase> query,
        PurchaseFilterRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(
            request.Currency))
        {
            return query;
        }

        var currency =
            request.Currency.Trim();

        return query.Where(purchase =>
            purchase.Currency.Equals(
                currency,
                StringComparison.OrdinalIgnoreCase));
    }


    private static IEnumerable<Purchase> ApplyAmountFilter(
        IEnumerable<Purchase> query,
        PurchaseFilterRequestDto request)
    {
        if (request.MinimumAmount.HasValue)
        {
            query = query.Where(purchase =>
                purchase.TotalAmount >=
                request.MinimumAmount.Value);
        }

        if (request.MaximumAmount.HasValue)
        {
            query = query.Where(purchase =>
                purchase.TotalAmount <=
                request.MaximumAmount.Value);
        }

        return query;
    }


    private static IEnumerable<Purchase> ApplyStatusFilter(
        IEnumerable<Purchase> query,
        PurchaseFilterRequestDto request)
    {
        if (!request.Status.HasValue)
        {
            return query;
        }

        return query.Where(purchase =>
            purchase.Status ==
            request.Status.Value);
    }


    // ================================================================
    // Pagination
    // ================================================================

    private static PurchaseListResponseDto
        CreatePagedResponse(
            IEnumerable<Purchase> query,
            PurchaseFilterRequestDto request)
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

        return new PurchaseListResponseDto
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }


    // ================================================================
    // Create Purchase
    // ================================================================

    public async Task<PurchaseResponseDto> CreateAsync(
        CreatePurchaseRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);


        // ============================================================
        // Validate Supplier
        // ============================================================

        var supplier = await _unitOfWork.Suppliers
            .GetByIdAsync(
                request.SupplierId,
                cancellationToken);

        if (supplier is null)
        {
            throw new KeyNotFoundException(
                $"Supplier with ID '{request.SupplierId}' was not found.");
        }

        if (!supplier.IsActive)
        {
            throw new InvalidOperationException(
                "Cannot create a purchase for an inactive supplier.");
        }


        // ============================================================
        // Generate Purchase Number
        // ============================================================

        var purchaseNumber =
            await GeneratePurchaseNumberAsync(
                cancellationToken);


        // ============================================================
        // Create Entity
        // ============================================================

        var purchaseDate = DateTime.SpecifyKind(
            request.PurchaseDate,
            DateTimeKind.Utc);

        var purchase = Purchase.Create(
            purchaseNumber,
            request.SupplierId,
            purchaseDate,
            request.InvoiceNumber,
            request.PurchaseOrderNumber,
            request.Description,
            request.TotalAmount,
            request.Currency);


        // ============================================================
        // Persist
        // ============================================================

        await _unitOfWork.Purchases.AddAsync(
            purchase,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);


        return MapToResponse(purchase);
    }


    // ================================================================
    // Update Purchase
    // ================================================================

    public async Task<PurchaseResponseDto> UpdateAsync(
        Guid id,
        UpdatePurchaseRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ValidateId(id);

        ArgumentNullException.ThrowIfNull(request);


        // ============================================================
        // Get Existing Purchase
        // ============================================================

        var purchase = await _unitOfWork.Purchases
            .GetByIdAsync(
                id,
                cancellationToken);

        if (purchase is null)
        {
            throw new KeyNotFoundException(
                $"Purchase with ID '{id}' was not found.");
        }


        // ============================================================
        // Validate Supplier
        // ============================================================

        var supplier = await _unitOfWork.Suppliers
            .GetByIdAsync(
                request.SupplierId,
                cancellationToken);

        if (supplier is null)
        {
            throw new KeyNotFoundException(
                $"Supplier with ID '{request.SupplierId}' was not found.");
        }

        if (!supplier.IsActive)
        {
            throw new InvalidOperationException(
                "Cannot assign an inactive supplier to a purchase.");
        }


        // ============================================================
        // Update Entity
        // ============================================================

        var purchaseDate = DateTime.SpecifyKind(
            request.PurchaseDate,
            DateTimeKind.Utc);

        purchase.Update(
            request.SupplierId,
            purchaseDate,
            request.InvoiceNumber,
            request.PurchaseOrderNumber,
            request.Description,
            request.TotalAmount,
            request.Currency);


        // ============================================================
        // Persist
        // ============================================================

        _unitOfWork.Purchases.Update(purchase);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);


        return MapToResponse(purchase);
    }


    // ================================================================
    // Delete Purchase
    // ================================================================

    public async Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        ValidateId(id);


        // ============================================================
        // Get Purchase With Assets
        // ============================================================

        var purchase = await _unitOfWork.Purchases
            .GetByIdWithDetailsAsync(
                id,
                cancellationToken);

        if (purchase is null)
        {
            throw new KeyNotFoundException(
                $"Purchase with ID '{id}' was not found.");
        }


        // ============================================================
        // Prevent Delete When Assets Exist
        // ============================================================

        if (purchase.Assets.Count > 0)
        {
            throw new InvalidOperationException(
                "A purchase cannot be deleted because it has associated assets.");
        }


        // ============================================================
        // Delete
        // ============================================================

        _unitOfWork.Purchases.Delete(purchase);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }


    // ================================================================
    // Generate Purchase Number
    // ================================================================

    private async Task<string> GeneratePurchaseNumberAsync(
        CancellationToken cancellationToken)
    {
        const string prefix = "PUR";

        var datePart =
            DateTime.UtcNow.ToString("yyyyMMdd");

        var purchaseNumber =
            $"{prefix}-{datePart}-{Guid.NewGuid():N}"[..20];

        while (await _unitOfWork.Purchases
            .GetByPurchaseNumberAsync(
                purchaseNumber,
                cancellationToken) is not null)
        {
            purchaseNumber =
                $"{prefix}-{datePart}-{Guid.NewGuid():N}"[..20];
        }

        return purchaseNumber;
    }


    // ================================================================
    // Mapping
    // ================================================================

    private static PurchaseResponseDto MapToResponse(
        Purchase purchase)
    {
        return new PurchaseResponseDto
        {
            Id = purchase.Id,

            PurchaseNumber =
                purchase.PurchaseNumber,

            SupplierId =
                purchase.SupplierId,

            SupplierName =
                purchase.Supplier?.Name ?? string.Empty,

            PurchaseDate =
                purchase.PurchaseDate,

            InvoiceNumber =
                purchase.InvoiceNumber,

            PurchaseOrderNumber =
                purchase.PurchaseOrderNumber,

            Description =
                purchase.Description,

            TotalAmount =
                purchase.TotalAmount,

            Currency =
                purchase.Currency,

            Status =
                purchase.Status,

            AssetCount =
                purchase.Assets.Count,

            IsActive =
                purchase.IsActive,

            CreatedAt =
                purchase.CreatedAt,

            UpdatedAt =
                purchase.UpdatedAt
        };
    }


    private static PurchaseDetailResponseDto
        MapToDetailResponse(Purchase purchase)
    {
        return new PurchaseDetailResponseDto
        {
            Id = purchase.Id,

            PurchaseNumber =
                purchase.PurchaseNumber,

            SupplierId =
                purchase.SupplierId,

            SupplierName =
                purchase.Supplier?.Name ?? string.Empty,

            PurchaseDate =
                purchase.PurchaseDate,

            InvoiceNumber =
                purchase.InvoiceNumber,

            PurchaseOrderNumber =
                purchase.PurchaseOrderNumber,

            Description =
                purchase.Description,

            TotalAmount =
                purchase.TotalAmount,

            Currency =
                purchase.Currency,

            Status =
                purchase.Status,

            IsActive =
                purchase.IsActive,

            CreatedAt =
                purchase.CreatedAt,

            UpdatedAt =
                purchase.UpdatedAt,

            Assets = purchase.Assets
                .Select(asset => new PurchaseAssetSummaryDto
                {
                    Id = asset.Id,
                    AssetTag = asset.AssetTag,
                    AssetName = asset.Name,
                    AssetCategoryId =
                        asset.AssetCategoryId,
                    SerialNumber =
                        asset.SerialNumber,
                    PurchaseCost =
                        asset.PurchaseCost,
                    IsActive =
                        asset.IsActive
                })
                .ToList()
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
                "Purchase ID is required.",
                nameof(id));
        }
    }
}