using UAMS.Application.DTOs.AssetDisposals.Requests;
using UAMS.Application.DTOs.AssetDisposals.Responses;
using UAMS.Application.Interfaces.Persistence;
using UAMS.Application.Interfaces.Services;
using UAMS.Domain.Entities.AssetDisposals;

namespace UAMS.Application.Services.AssetDisposals;

public class AssetDisposalService : IAssetDisposalService
{
    private readonly IUnitOfWork _unitOfWork;

    public AssetDisposalService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    // ============================================================
    // GET BY ID
    // ============================================================

    public async Task<AssetDisposalResponseDto> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var disposal = await _unitOfWork.AssetDisposals
            .GetByIdAsync(id, cancellationToken);

        if (disposal is null)
            throw new KeyNotFoundException(
                $"Asset disposal with ID '{id}' was not found.");

        return MapToResponse(disposal);
    }


    // ============================================================
    // GET DETAILS
    // ============================================================

    public async Task<AssetDisposalDetailResponseDto> GetDetailsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var disposal = await _unitOfWork.AssetDisposals
            .GetByIdAsync(id, cancellationToken);

        if (disposal is null)
            throw new KeyNotFoundException(
                $"Asset disposal with ID '{id}' was not found.");

        return MapToDetailResponse(disposal);
    }


    // ============================================================
    // GET BY DISPOSAL NUMBER
    // ============================================================

    public async Task<AssetDisposalResponseDto?> GetByDisposalNumberAsync(
        string disposalNumber,
        CancellationToken cancellationToken = default)
    {
        var disposal = await _unitOfWork.AssetDisposals
            .GetByDisposalNumberAsync(
                disposalNumber,
                cancellationToken);

        return disposal is null
            ? null
            : MapToResponse(disposal);
    }


    // ============================================================
    // GET ALL / FILTER / PAGINATION
    // ============================================================

    public async Task<AssetDisposalListResponseDto> GetAllAsync(
        AssetDisposalFilterRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var query = await _unitOfWork.AssetDisposals
            .GetAllAsync(cancellationToken);

        var filtered = query.AsEnumerable();

        // ------------------------------------------------------------
        // Search
        // ------------------------------------------------------------

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var search = request.SearchTerm.Trim();

            filtered = filtered.Where(x =>
                x.DisposalNumber.Contains(
                    search,
                    StringComparison.OrdinalIgnoreCase) ||

                x.Reason.Contains(
                    search,
                    StringComparison.OrdinalIgnoreCase) ||

                (x.Remarks != null &&
                 x.Remarks.Contains(
                     search,
                     StringComparison.OrdinalIgnoreCase)));
        }

        // ------------------------------------------------------------
        // Asset
        // ------------------------------------------------------------

        if (request.AssetId.HasValue)
        {
            filtered = filtered.Where(x =>
                x.AssetId == request.AssetId.Value);
        }

        // ------------------------------------------------------------
        // Maintenance
        // ------------------------------------------------------------

        if (request.MaintenanceId.HasValue)
        {
            filtered = filtered.Where(x =>
                x.MaintenanceId == request.MaintenanceId.Value);
        }

        // ------------------------------------------------------------
        // Requested By
        // ------------------------------------------------------------

        if (request.RequestedById.HasValue)
        {
            filtered = filtered.Where(x =>
                x.RequestedById == request.RequestedById.Value);
        }

        // ------------------------------------------------------------
        // Approved By
        // ------------------------------------------------------------

        if (request.ApprovedById.HasValue)
        {
            filtered = filtered.Where(x =>
                x.ApprovedById == request.ApprovedById.Value);
        }

        // ------------------------------------------------------------
        // Completed By
        // ------------------------------------------------------------

        if (request.CompletedById.HasValue)
        {
            filtered = filtered.Where(x =>
                x.CompletedById == request.CompletedById.Value);
        }

        // ------------------------------------------------------------
        // Status
        // ------------------------------------------------------------

        if (request.Status.HasValue)
        {
            filtered = filtered.Where(x =>
                x.Status == request.Status.Value);
        }

        // ------------------------------------------------------------
        // Disposal Method
        // ------------------------------------------------------------

        if (request.DisposalMethod.HasValue)
        {
            filtered = filtered.Where(x =>
                x.DisposalMethod == request.DisposalMethod.Value);
        }

        // ------------------------------------------------------------
        // Requested Date
        // ------------------------------------------------------------

        if (request.RequestedFromDate.HasValue)
        {
            filtered = filtered.Where(x =>
                x.RequestedDate >= request.RequestedFromDate.Value);
        }

        if (request.RequestedToDate.HasValue)
        {
            filtered = filtered.Where(x =>
                x.RequestedDate <= request.RequestedToDate.Value);
        }

        // ------------------------------------------------------------
        // Approved Date
        // ------------------------------------------------------------

        if (request.ApprovedFromDate.HasValue)
        {
            filtered = filtered.Where(x =>
                x.ApprovedDate >= request.ApprovedFromDate.Value);
        }

        if (request.ApprovedToDate.HasValue)
        {
            filtered = filtered.Where(x =>
                x.ApprovedDate <= request.ApprovedToDate.Value);
        }

        // ------------------------------------------------------------
        // Disposal Date
        // ------------------------------------------------------------

        if (request.DisposalFromDate.HasValue)
        {
            filtered = filtered.Where(x =>
                x.DisposalDate >= request.DisposalFromDate.Value);
        }

        if (request.DisposalToDate.HasValue)
        {
            filtered = filtered.Where(x =>
                x.DisposalDate <= request.DisposalToDate.Value);
        }

        // ------------------------------------------------------------
        // Ordering
        // ------------------------------------------------------------

        var ordered = filtered
            .OrderByDescending(x => x.RequestedDate)
            .ToList();

        var totalCount = ordered.Count;

        var items = ordered
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(MapToResponse)
            .ToList();

        var totalPages = (int)Math.Ceiling(
            totalCount / (double)request.PageSize);

        return new AssetDisposalListResponseDto
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            HasPreviousPage = request.PageNumber > 1,
            HasNextPage = request.PageNumber < totalPages
        };
    }


    // ============================================================
    // CREATE
    // ============================================================

    public async Task<AssetDisposalResponseDto> CreateAsync(
        CreateAssetDisposalRequestDto request,
        Guid requestedById,
        CancellationToken cancellationToken = default)
    {
        if (requestedById == Guid.Empty)
            throw new InvalidOperationException(
                "The requesting user could not be identified.");

        var asset = await _unitOfWork.Assets
            .GetByIdAsync(
                request.AssetId,
                cancellationToken);

        if (asset is null)
            throw new KeyNotFoundException(
                $"Asset with ID '{request.AssetId}' was not found.");

        if (request.MaintenanceId.HasValue)
        {
            var maintenance = await _unitOfWork.MaintenanceRequests
                .GetByIdAsync(
                    request.MaintenanceId.Value,
                    cancellationToken);

            if (maintenance is null)
                throw new KeyNotFoundException(
                    $"Maintenance request with ID '{request.MaintenanceId}' was not found.");
        }

        var disposalNumber =
            await GenerateDisposalNumberAsync(cancellationToken);

        var disposal = AssetDisposal.Create(
            disposalNumber,
            request.AssetId,
            request.MaintenanceId,
            requestedById,
            request.Reason,
            request.BookValue,
            request.EstimatedValue,
            request.Remarks);

        await _unitOfWork.AssetDisposals
            .AddAsync(disposal, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToResponse(disposal);
    }


    // ============================================================
    // UPDATE
    // ============================================================

    public async Task<AssetDisposalResponseDto> UpdateAsync(
        Guid id,
        UpdateAssetDisposalRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var disposal = await _unitOfWork.AssetDisposals
            .GetByIdAsync(id, cancellationToken);

        if (disposal is null)
            throw new KeyNotFoundException(
                $"Asset disposal with ID '{id}' was not found.");

        if (request.MaintenanceId.HasValue)
        {
            var maintenance = await _unitOfWork.MaintenanceRequests
                .GetByIdAsync(
                    request.MaintenanceId.Value,
                    cancellationToken);

            if (maintenance is null)
                throw new KeyNotFoundException(
                    $"Maintenance request with ID '{request.MaintenanceId}' was not found.");
        }

        disposal.Update(
            request.MaintenanceId,
            request.Reason,
            request.BookValue,
            request.EstimatedValue,
            request.Remarks);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToResponse(disposal);
    }


    // ============================================================
    // START REVIEW
    // ============================================================

    public async Task StartReviewAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var disposal = await _unitOfWork.AssetDisposals
            .GetByIdAsync(id, cancellationToken);

        if (disposal is null)
            throw new KeyNotFoundException(
                $"Asset disposal with ID '{id}' was not found.");

        disposal.StartReview();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }


    // ============================================================
    // APPROVE
    // ============================================================

    public async Task<AssetDisposalResponseDto> ApproveAsync(
        Guid id,
        ApproveAssetDisposalRequestDto request,
        Guid approvedById,
        CancellationToken cancellationToken = default)
    {
        var disposal = await _unitOfWork.AssetDisposals
            .GetByIdAsync(id, cancellationToken);

        if (disposal is null)
            throw new KeyNotFoundException(
                $"Asset disposal with ID '{id}' was not found.");

        disposal.Approve(
            approvedById,
            request.DisposalMethod,
            request.Remarks);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToResponse(disposal);
    }


    // ============================================================
    // REJECT
    // ============================================================

    public async Task<AssetDisposalResponseDto> RejectAsync(
        Guid id,
        RejectAssetDisposalRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var disposal = await _unitOfWork.AssetDisposals
            .GetByIdAsync(id, cancellationToken);

        if (disposal is null)
            throw new KeyNotFoundException(
                $"Asset disposal with ID '{id}' was not found.");

        disposal.Reject(request.Reason);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToResponse(disposal);
    }


    // ============================================================
    // COMPLETE
    // ============================================================

    public async Task<AssetDisposalResponseDto> CompleteAsync(
        Guid id,
        CompleteAssetDisposalRequestDto request,
        Guid completedById,
        CancellationToken cancellationToken = default)
    {
        var disposal = await _unitOfWork.AssetDisposals
            .GetByIdAsync(id, cancellationToken);

        if (disposal is null)
            throw new KeyNotFoundException(
                $"Asset disposal with ID '{id}' was not found.");

        disposal.Complete(
            completedById,
            request.DisposalMethod,
            request.DisposalValue,
            request.Remarks);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToResponse(disposal);
    }


    // ============================================================
    // GENERATE DISPOSAL NUMBER
    // ============================================================

    private async Task<string> GenerateDisposalNumberAsync(
        CancellationToken cancellationToken)
    {
        var prefix = $"DIS-{DateTime.UtcNow:yyyyMMdd}";

        var disposalNumber = prefix;

        var counter = 1;

        while (await _unitOfWork.AssetDisposals
                   .GetByDisposalNumberAsync(
                       disposalNumber,
                       cancellationToken) is not null)
        {
            disposalNumber =
                $"{prefix}-{counter:D4}";

            counter++;
        }

        return disposalNumber;
    }


    // ============================================================
    // MAP RESPONSE
    // ============================================================

    private static AssetDisposalResponseDto MapToResponse(
        AssetDisposal disposal)
    {
        return new AssetDisposalResponseDto
        {
            Id = disposal.Id,

            DisposalNumber =
                disposal.DisposalNumber,

            AssetId =
                disposal.AssetId,

            MaintenanceId =
                disposal.MaintenanceId,

            RequestedById =
                disposal.RequestedById,

            ApprovedById =
                disposal.ApprovedById,

            CompletedById =
                disposal.CompletedById,

            DisposalMethod =
                disposal.DisposalMethod,

            Reason =
                disposal.Reason,

            BookValue =
                disposal.BookValue,

            EstimatedValue =
                disposal.EstimatedValue,

            DisposalValue =
                disposal.DisposalValue,

            RequestedDate =
                disposal.RequestedDate,

            ApprovedDate =
                disposal.ApprovedDate,

            DisposalDate =
                disposal.DisposalDate,

            Remarks =
                disposal.Remarks,

            Status =
                disposal.Status,

            IsActive =
                disposal.IsActive
        };
    }


    // ============================================================
    // MAP DETAIL RESPONSE
    // ============================================================

    private static AssetDisposalDetailResponseDto MapToDetailResponse(
        AssetDisposal disposal)
    {
        return new AssetDisposalDetailResponseDto
        {
            Id = disposal.Id,

            DisposalNumber =
                disposal.DisposalNumber,

            AssetId =
                disposal.AssetId,

            MaintenanceId =
                disposal.MaintenanceId,

            RequestedById =
                disposal.RequestedById,

            ApprovedById =
                disposal.ApprovedById,

            CompletedById =
                disposal.CompletedById,

            DisposalMethod =
                disposal.DisposalMethod,

            Reason =
                disposal.Reason,

            BookValue =
                disposal.BookValue,

            EstimatedValue =
                disposal.EstimatedValue,

            DisposalValue =
                disposal.DisposalValue,

            RequestedDate =
                disposal.RequestedDate,

            ApprovedDate =
                disposal.ApprovedDate,

            DisposalDate =
                disposal.DisposalDate,

            Remarks =
                disposal.Remarks,

            Status =
                disposal.Status,

            IsActive =
                disposal.IsActive,

            CreatedAt =
                disposal.CreatedAt,

            CreatedBy =
                disposal.CreatedBy,

            UpdatedAt =
                disposal.UpdatedAt,

            UpdatedBy =
                disposal.UpdatedBy
        };
    }
}