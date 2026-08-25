using UAMS.Application.DTOs.AssetReturns.Requests;
using UAMS.Application.DTOs.AssetReturns.Responses;
using UAMS.Application.Interfaces.Persistence;
using UAMS.Application.Interfaces.Services;
using UAMS.Domain.Entities.AssetReturns;
using UAMS.Domain.Enums;

namespace UAMS.Application.Services.AssetReturns;

public class AssetReturnService : IAssetReturnService
{
    private readonly IUnitOfWork _unitOfWork;

    public AssetReturnService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork
            ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    // ================================================================
    // Create
    // ================================================================

    public async Task<AssetReturnResponseDto> CreateAsync(
        CreateAssetReturnRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var asset = await _unitOfWork.Assets.GetByIdAsync(
            request.AssetId,
            cancellationToken);

        if (asset is null)
        {
            throw new KeyNotFoundException(
                $"Asset with ID '{request.AssetId}' was not found.");
        }

        var assignment = await _unitOfWork.AssetAssignments.GetByIdAsync(
            request.AssetAssignmentId,
            cancellationToken);

        if (assignment is null)
        {
            throw new KeyNotFoundException(
                $"Asset assignment with ID '{request.AssetAssignmentId}' was not found.");
        }

        var returnedBy = await _unitOfWork.Users.GetByIdAsync(
            request.ReturnedById,
            cancellationToken);

        if (returnedBy is null)
        {
            throw new KeyNotFoundException(
                $"User with ID '{request.ReturnedById}' was not found.");
        }

        var receivedBy = await _unitOfWork.Users.GetByIdAsync(
            request.ReceivedById,
            cancellationToken);

        if (receivedBy is null)
        {
            throw new KeyNotFoundException(
                $"User with ID '{request.ReceivedById}' was not found.");
        }

        var existingReturns =
            await _unitOfWork.AssetReturns.GetByAssetAssignmentIdAsync(
                request.AssetAssignmentId,
                cancellationToken);

        var hasOpenReturn = existingReturns.Any(x =>
            x.Status != AssetReturnStatus.Completed &&
            x.Status != AssetReturnStatus.Cancelled);

        if (hasOpenReturn)
        {
            throw new InvalidOperationException(
                "There is already an active asset return for this assignment.");
        }

        var returnNumber = await GenerateReturnNumberAsync(
            cancellationToken);

        var assetReturn = AssetReturn.Create(
            returnNumber,
            request.AssetId,
            request.AssetAssignmentId,
            request.ReturnedById,
            request.ReceivedById,
            request.ReturnDate,
            request.ReturnLocation,
            request.Condition,
            request.InspectionNotes,
            request.Remarks);

        await _unitOfWork.AssetReturns.AddAsync(
            assetReturn,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(assetReturn);
    }

    // ================================================================
    // Get By Id
    // ================================================================

    public async Task<AssetReturnDetailResponseDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.AssetReturns.GetByIdAsync(
            id,
            cancellationToken);

        if (entity is null)
        {
            return null;
        }

        return await MapToDetailResponseAsync(
            entity,
            cancellationToken);
    }

    // ================================================================
    // Get By Return Number
    // ================================================================

    public async Task<AssetReturnResponseDto?> GetByReturnNumberAsync(
        string returnNumber,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(returnNumber))
        {
            return null;
        }

        var entity =
            await _unitOfWork.AssetReturns.GetByReturnNumberAsync(
                returnNumber.Trim(),
                cancellationToken);

        return entity is null
            ? null
            : MapToResponse(entity);
    }

    // ================================================================
    // Get By Asset
    // ================================================================

    public async Task<IReadOnlyList<AssetReturnResponseDto>>
        GetByAssetIdAsync(
            Guid assetId,
            CancellationToken cancellationToken = default)
    {
        var entities =
            await _unitOfWork.AssetReturns.GetByAssetIdAsync(
                assetId,
                cancellationToken);

        return entities
            .Select(MapToResponse)
            .ToList();
    }

    // ================================================================
    // Get By Asset Assignment
    // ================================================================

    public async Task<IReadOnlyList<AssetReturnResponseDto>>
        GetByAssetAssignmentIdAsync(
            Guid assetAssignmentId,
            CancellationToken cancellationToken = default)
    {
        var entities =
            await _unitOfWork.AssetReturns.GetByAssetAssignmentIdAsync(
                assetAssignmentId,
                cancellationToken);

        return entities
            .Select(MapToResponse)
            .ToList();
    }

    // ================================================================
    // Get By Employee
    // ================================================================

    public async Task<IReadOnlyList<AssetReturnResponseDto>>
        GetByEmployeeIdAsync(
            Guid employeeId,
            CancellationToken cancellationToken = default)
    {
        var entities =
            await _unitOfWork.AssetReturns.GetByEmployeeIdAsync(
                employeeId,
                cancellationToken);

        return entities
            .Select(MapToResponse)
            .ToList();
    }

    // ================================================================
    // Get By Received By
    // ================================================================

    public async Task<IReadOnlyList<AssetReturnResponseDto>>
        GetByReceivedByIdAsync(
            Guid receivedById,
            CancellationToken cancellationToken = default)
    {
        var entities =
            await _unitOfWork.AssetReturns.GetByReceivedByIdAsync(
                receivedById,
                cancellationToken);

        return entities
            .Select(MapToResponse)
            .ToList();
    }

    // ================================================================
    // Get By Inspector
    // ================================================================

    public async Task<IReadOnlyList<AssetReturnResponseDto>>
        GetByInspectedByIdAsync(
            Guid inspectedById,
            CancellationToken cancellationToken = default)
    {
        var entities =
            await _unitOfWork.AssetReturns.GetByInspectedByIdAsync(
                inspectedById,
                cancellationToken);

        return entities
            .Select(MapToResponse)
            .ToList();
    }

    // ================================================================
    // Get By Status
    // ================================================================

    public async Task<IReadOnlyList<AssetReturnResponseDto>>
        GetByStatusAsync(
            AssetReturnStatus status,
            CancellationToken cancellationToken = default)
    {
        var entities =
            await _unitOfWork.AssetReturns.GetByStatusAsync(
                status,
                cancellationToken);

        return entities
            .Select(MapToResponse)
            .ToList();
    }

    // ================================================================
    // Get Pending Inspection
    // ================================================================

    public async Task<IReadOnlyList<AssetReturnResponseDto>>
        GetPendingInspectionAsync(
            CancellationToken cancellationToken = default)
    {
        var entities =
            await _unitOfWork.AssetReturns.GetPendingInspectionAsync(
                cancellationToken);

        return entities
            .Select(MapToResponse)
            .ToList();
    }

    // ================================================================
    // Get With Damage
    // ================================================================

    public async Task<IReadOnlyList<AssetReturnResponseDto>>
        GetWithDamageAsync(
            CancellationToken cancellationToken = default)
    {
        var entities =
            await _unitOfWork.AssetReturns.GetWithDamageAsync(
                cancellationToken);

        return entities
            .Select(MapToResponse)
            .ToList();
    }

    // ================================================================
    // Filter / Search / Pagination
    // ================================================================

    public async Task<AssetReturnListResponseDto> FilterAsync(
        AssetReturnFilterRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var pageNumber = NormalizePageNumber(request.PageNumber);
        var pageSize = NormalizePageSize(request.PageSize);

        var filterRequest = new AssetReturnFilterRequestDto
        {
            ReturnNumber = request.ReturnNumber?.Trim(),

            AssetId = request.AssetId,

            AssetAssignmentId = request.AssetAssignmentId,

            ReturnedById = request.ReturnedById,

            ReceivedById = request.ReceivedById,

            InspectedById = request.InspectedById,

            DamageReportId = request.DamageReportId,

            Condition = request.Condition,

            DamageFound = request.DamageFound,

            Status = request.Status,

            ReturnDateFrom = request.ReturnDateFrom,

            ReturnDateTo = request.ReturnDateTo,

            PageNumber = pageNumber,

            PageSize = pageSize,

            SearchTerm = request.SearchTerm?.Trim()
        };

        var result = await _unitOfWork.AssetReturns.FilterAsync(
            filterRequest,
            cancellationToken);

        var totalPages = CalculateTotalPages(
            result.TotalCount,
            pageSize);

        var items = result.Items
            .Select(MapToResponse)
            .ToList();

        return new AssetReturnListResponseDto
        {
            Items = items,

            PageNumber = pageNumber,

            PageSize = pageSize,

            TotalCount = result.TotalCount,

            TotalPages = totalPages,

            HasPreviousPage = pageNumber > 1,

            HasNextPage = pageNumber < totalPages
        };
    }

    // ================================================================
    // Update
    // ================================================================

    public async Task<AssetReturnResponseDto> UpdateAsync(
        Guid id,
        UpdateAssetReturnRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entity = await GetRequiredEntityAsync(
            id,
            cancellationToken);

        entity.Update(
            request.ReturnDate,
            request.ReturnLocation,
            request.Condition,
            request.InspectionNotes,
            request.Remarks);

        _unitOfWork.AssetReturns.Update(entity);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(entity);
    }

    // ================================================================
    // Inspect
    // ================================================================

    public async Task<AssetReturnResponseDto> InspectAsync(
        Guid id,
        InspectAssetReturnRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entity = await GetRequiredEntityAsync(
            id,
            cancellationToken);

        await ValidateInspectionAsync(
            request,
            cancellationToken);

        entity.Inspect(
            request.InspectedById,
            request.InspectionDate,
            request.DamageFound,
            request.InspectionNotes,
            request.DamageReportId,
            request.Remarks);

        _unitOfWork.AssetReturns.Update(entity);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(entity);
    }

    // ================================================================
    // Complete
    // ================================================================

    public async Task<AssetReturnResponseDto> CompleteAsync(
        Guid id,
        CompleteAssetReturnRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entity = await GetRequiredEntityAsync(
            id,
            cancellationToken);

        entity.Complete(request.Remarks);

        _unitOfWork.AssetReturns.Update(entity);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(entity);
    }

    // ================================================================
    // Cancel
    // ================================================================

    public async Task<AssetReturnResponseDto> CancelAsync(
        Guid id,
        CancelAssetReturnRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entity = await GetRequiredEntityAsync(
            id,
            cancellationToken);

        entity.Cancel(request.Reason);

        _unitOfWork.AssetReturns.Update(entity);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(entity);
    }

    // ================================================================
    // Helpers - Pagination
    // ================================================================

    private static int NormalizePageNumber(int pageNumber)
    {
        return pageNumber < 1
            ? 1
            : pageNumber;
    }

    private static int NormalizePageSize(int pageSize)
    {
        if (pageSize < 1)
        {
            return 20;
        }

        if (pageSize > 100)
        {
            return 100;
        }

        return pageSize;
    }

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
    // Helpers - Validation
    // ================================================================

    private async Task ValidateInspectionAsync(
        InspectAssetReturnRequestDto request,
        CancellationToken cancellationToken)
    {
        if (request.DamageFound)
        {
            await ValidateDamageReportAsync(
                request.DamageReportId,
                cancellationToken);
        }

        var inspector = await _unitOfWork.Users.GetByIdAsync(
            request.InspectedById,
            cancellationToken);

        if (inspector is null)
        {
            throw new KeyNotFoundException(
                $"Inspector with ID '{request.InspectedById}' was not found.");
        }
    }

    private async Task ValidateDamageReportAsync(
        Guid? damageReportId,
        CancellationToken cancellationToken)
    {
        if (!damageReportId.HasValue ||
            damageReportId.Value == Guid.Empty)
        {
            throw new InvalidOperationException(
                "A damage report is required when damage is found.");
        }

        var damageReport =
            await _unitOfWork.DamageReports.GetByIdAsync(
                damageReportId.Value,
                cancellationToken);

        if (damageReport is null)
        {
            throw new KeyNotFoundException(
                $"Damage report with ID '{damageReportId.Value}' was not found.");
        }
    }

    // ================================================================
    // Helpers - Entity
    // ================================================================

    private async Task<AssetReturn> GetRequiredEntityAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.AssetReturns.GetByIdAsync(
            id,
            cancellationToken);

        if (entity is null)
        {
            throw new KeyNotFoundException(
                $"Asset return with ID '{id}' was not found.");
        }

        return entity;
    }

    // ================================================================
    // Helpers - Return Number
    // ================================================================

    private async Task<string> GenerateReturnNumberAsync(
        CancellationToken cancellationToken)
    {
        const string prefix = "RET";

        for (var attempt = 0; attempt < 10; attempt++)
        {
            var number =
                $"{prefix}-{DateTime.UtcNow:yyyyMMddHHmmssfff}-{Random.Shared.Next(100, 999)}";

            var exists =
                await _unitOfWork.AssetReturns.GetByReturnNumberAsync(
                    number,
                    cancellationToken);

            if (exists is null)
            {
                return number;
            }
        }

        throw new InvalidOperationException(
            "Unable to generate a unique asset return number.");
    }

    // ================================================================
    // Helpers - Mapping
    // ================================================================

    private static AssetReturnResponseDto MapToResponse(
        AssetReturn entity)
    {
        return new AssetReturnResponseDto
        {
            Id = entity.Id,

            ReturnNumber = entity.ReturnNumber,

            AssetId = entity.AssetId,

            AssetAssignmentId = entity.AssetAssignmentId,

            ReturnedById = entity.ReturnedById,

            ReceivedById = entity.ReceivedById,

            ReturnDate = entity.ReturnDate,

            ReturnLocation = entity.ReturnLocation,

            Condition = entity.Condition,

            InspectionNotes = entity.InspectionNotes,

            InspectedById = entity.InspectedById,

            InspectionDate = entity.InspectionDate,

            DamageFound = entity.DamageFound,

            DamageReportId = entity.DamageReportId,

            Remarks = entity.Remarks,

            Status = entity.Status
        };
    }

    // ================================================================
    // Map To Detail Response
    // ================================================================

    private async Task<AssetReturnDetailResponseDto>
        MapToDetailResponseAsync(
            AssetReturn entity,
            CancellationToken cancellationToken)
    {
        var asset = await _unitOfWork.Assets.GetByIdAsync(
            entity.AssetId,
            cancellationToken);

        if (asset is null)
        {
            throw new KeyNotFoundException(
                $"Asset with ID '{entity.AssetId}' was not found.");
        }

        var assignment =
            await _unitOfWork.AssetAssignments.GetByIdAsync(
                entity.AssetAssignmentId,
                cancellationToken);

        if (assignment is null)
        {
            throw new KeyNotFoundException(
                $"Asset assignment with ID '{entity.AssetAssignmentId}' was not found.");
        }

        var returnedBy =
            await _unitOfWork.Users.GetByIdAsync(
                entity.ReturnedById,
                cancellationToken);

        if (returnedBy is null)
        {
            throw new KeyNotFoundException(
                $"User with ID '{entity.ReturnedById}' was not found.");
        }

        var receivedBy =
            await _unitOfWork.Users.GetByIdAsync(
                entity.ReceivedById,
                cancellationToken);

        if (receivedBy is null)
        {
            throw new KeyNotFoundException(
                $"User with ID '{entity.ReceivedById}' was not found.");
        }

        var inspectedBy = entity.InspectedById.HasValue
            ? await _unitOfWork.Users.GetByIdAsync(
                entity.InspectedById.Value,
                cancellationToken)
            : null;

        var damageReport = entity.DamageReportId.HasValue
            ? await _unitOfWork.DamageReports.GetByIdAsync(
                entity.DamageReportId.Value,
                cancellationToken)
            : null;

        return new AssetReturnDetailResponseDto
        {
            Id = entity.Id,

            ReturnNumber = entity.ReturnNumber,

            // ========================================================
            // Asset
            // ========================================================

            AssetId = entity.AssetId,

            AssetTag = asset.AssetTag,

            AssetName = asset.Name,

            // ========================================================
            // Assignment
            // ========================================================

            AssetAssignmentId = entity.AssetAssignmentId,

            AssignmentNumber = assignment.AssignmentNumber,

            // ========================================================
            // Return
            // ========================================================

            ReturnedById = entity.ReturnedById,

            ReturnedByName = returnedBy.FullName,

            ReceivedById = entity.ReceivedById,

            ReceivedByName = receivedBy.FullName,

            ReturnDate = entity.ReturnDate,

            ReturnLocation = entity.ReturnLocation,

            Condition = entity.Condition,

            // ========================================================
            // Inspection
            // ========================================================

            InspectedById = entity.InspectedById,

            InspectedByName = inspectedBy?.FullName,

            InspectionDate = entity.InspectionDate,

            InspectionNotes = entity.InspectionNotes,

            // ========================================================
            // Damage
            // ========================================================

            DamageFound = entity.DamageFound,

            DamageReportId = entity.DamageReportId,

            DamageReportNumber = damageReport?.ReportNumber,

            // ========================================================
            // Status
            // ========================================================

            Status = entity.Status,

            Remarks = entity.Remarks,

            // ========================================================
            // Audit
            // ========================================================

            CreatedAt = entity.CreatedAt,

            UpdatedAt = entity.UpdatedAt
        };
    }
}