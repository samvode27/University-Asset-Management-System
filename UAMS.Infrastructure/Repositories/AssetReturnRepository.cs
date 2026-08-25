using Microsoft.EntityFrameworkCore;
using UAMS.Application.Interfaces.Repositories;
using UAMS.Application.DTOs.AssetReturns.Requests;
using UAMS.Domain.Entities.AssetReturns;
using UAMS.Domain.Enums;
using UAMS.Infrastructure.Persistence;


namespace UAMS.Infrastructure.Repositories;

public class AssetReturnRepository
    : GenericRepository<AssetReturn>, IAssetReturnRepository
{
    public AssetReturnRepository(UAMSDbContext context)
        : base(context)
    {
    }


// ================================================================
// Filter / Search / Pagination
// ================================================================

public virtual async Task<(
    IReadOnlyList<AssetReturn> Items,
    int TotalCount)>
    FilterAsync(
        AssetReturnFilterRequestDto request,
        CancellationToken cancellationToken = default)
{
    IQueryable<AssetReturn> query = DbSet
        .AsNoTracking();

    // ============================================================
    // Return Number
    // ============================================================

    if (!string.IsNullOrWhiteSpace(request.ReturnNumber))
    {
        var returnNumber = request.ReturnNumber.Trim();

        query = query.Where(returnItem =>
            returnItem.ReturnNumber.Contains(returnNumber));
    }

    // ============================================================
    // Asset
    // ============================================================

    if (request.AssetId.HasValue)
    {
        query = query.Where(returnItem =>
            returnItem.AssetId == request.AssetId.Value);
    }

    // ============================================================
    // Asset Assignment
    // ============================================================

    if (request.AssetAssignmentId.HasValue)
    {
        query = query.Where(returnItem =>
            returnItem.AssetAssignmentId ==
            request.AssetAssignmentId.Value);
    }

    // ============================================================
    // Returned By
    // ============================================================

    if (request.ReturnedById.HasValue)
    {
        query = query.Where(returnItem =>
            returnItem.ReturnedById ==
            request.ReturnedById.Value);
    }

    // ============================================================
    // Received By
    // ============================================================

    if (request.ReceivedById.HasValue)
    {
        query = query.Where(returnItem =>
            returnItem.ReceivedById ==
            request.ReceivedById.Value);
    }

    // ============================================================
    // Inspected By
    // ============================================================

    if (request.InspectedById.HasValue)
    {
        query = query.Where(returnItem =>
            returnItem.InspectedById ==
            request.InspectedById.Value);
    }

    // ============================================================
    // Damage Report
    // ============================================================

    if (request.DamageReportId.HasValue)
    {
        query = query.Where(returnItem =>
            returnItem.DamageReportId ==
            request.DamageReportId.Value);
    }

    // ============================================================
    // Condition
    // ============================================================

    if (request.Condition.HasValue)
    {
        query = query.Where(returnItem =>
            returnItem.Condition ==
            request.Condition.Value);
    }

    // ============================================================
    // Damage Found
    // ============================================================

    if (request.DamageFound.HasValue)
    {
        query = query.Where(returnItem =>
            returnItem.DamageFound ==
            request.DamageFound.Value);
    }

    // ============================================================
    // Status
    // ============================================================

    if (request.Status.HasValue)
    {
        query = query.Where(returnItem =>
            returnItem.Status ==
            request.Status.Value);
    }

    // ============================================================
    // Return Date From
    // ============================================================

    if (request.ReturnDateFrom.HasValue)
    {
        query = query.Where(returnItem =>
            returnItem.ReturnDate >=
            request.ReturnDateFrom.Value);
    }

    // ============================================================
    // Return Date To
    // ============================================================

    if (request.ReturnDateTo.HasValue)
    {
        query = query.Where(returnItem =>
            returnItem.ReturnDate <=
            request.ReturnDateTo.Value);
    }

    // ============================================================
    // Search Term
    // ============================================================

    if (!string.IsNullOrWhiteSpace(request.SearchTerm))
    {
        var searchTerm = request.SearchTerm.Trim();

        query = query.Where(returnItem =>
            returnItem.ReturnNumber.Contains(searchTerm) ||
            (returnItem.ReturnLocation != null &&
             returnItem.ReturnLocation.Contains(searchTerm)) ||
            (returnItem.InspectionNotes != null &&
             returnItem.InspectionNotes.Contains(searchTerm)) ||
            (returnItem.Remarks != null &&
             returnItem.Remarks.Contains(searchTerm)));
    }

    // ============================================================
    // Total Count
    // ============================================================

    var totalCount = await query.CountAsync(
        cancellationToken);

    // ============================================================
    // Pagination
    // ============================================================

    var pageNumber = request.PageNumber < 1
        ? 1
        : request.PageNumber;

    var pageSize = request.PageSize < 1
        ? 20
        : Math.Min(request.PageSize, 100);

    var skip = (pageNumber - 1) * pageSize;

    var items = await query
        .OrderByDescending(returnItem =>
            returnItem.ReturnDate)
        .ThenByDescending(returnItem =>
            returnItem.CreatedAt)
        .Skip(skip)
        .Take(pageSize)
        .ToListAsync(cancellationToken);

    return (items, totalCount);
}

    // ================================================================
    // Get Return By Return Number
    // ================================================================

    public virtual async Task<AssetReturn?>
        GetByReturnNumberAsync(
            string returnNumber,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                returnItem =>
                    returnItem.ReturnNumber == returnNumber,
                cancellationToken);
    }


    // ================================================================
    // Get Returns By Asset
    // ================================================================

    public virtual async Task<IReadOnlyList<AssetReturn>>
        GetByAssetIdAsync(
            Guid assetId,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(returnItem =>
                returnItem.AssetId == assetId)
            .OrderByDescending(returnItem =>
                returnItem.ReturnDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Returns By Asset Assignment
    // ================================================================

    public virtual async Task<IReadOnlyList<AssetReturn>>
        GetByAssetAssignmentIdAsync(
            Guid assetAssignmentId,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(returnItem =>
                returnItem.AssetAssignmentId == assetAssignmentId)
            .OrderByDescending(returnItem =>
                returnItem.ReturnDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Returns By Employee
    // ================================================================

    public virtual async Task<IReadOnlyList<AssetReturn>>
        GetByEmployeeIdAsync(
            Guid employeeId,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(returnItem =>
                returnItem.ReturnedById == employeeId)
            .OrderByDescending(returnItem =>
                returnItem.ReturnDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Returns Received By User
    // ================================================================

    public virtual async Task<IReadOnlyList<AssetReturn>>
        GetByReceivedByIdAsync(
            Guid receivedById,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(returnItem =>
                returnItem.ReceivedById == receivedById)
            .OrderByDescending(returnItem =>
                returnItem.ReturnDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Returns Inspected By User
    // ================================================================

    public virtual async Task<IReadOnlyList<AssetReturn>>
        GetByInspectedByIdAsync(
            Guid inspectedById,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(returnItem =>
                returnItem.InspectedById == inspectedById)
            .OrderByDescending(returnItem =>
                returnItem.InspectionDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Returns By Status
    // ================================================================

    public virtual async Task<IReadOnlyList<AssetReturn>>
        GetByStatusAsync(
            AssetReturnStatus status,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(returnItem =>
                returnItem.Status == status)
            .OrderByDescending(returnItem =>
                returnItem.ReturnDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Returns Pending Inspection
    // ================================================================

    public virtual async Task<IReadOnlyList<AssetReturn>>
        GetPendingInspectionAsync(
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(returnItem =>
                returnItem.Status ==
                AssetReturnStatus.PendingInspection)
            .OrderBy(returnItem =>
                returnItem.ReturnDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Returns With Damage
    // ================================================================

    public virtual async Task<IReadOnlyList<AssetReturn>>
        GetWithDamageAsync(
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(returnItem =>
                returnItem.DamageFound)
            .OrderByDescending(returnItem =>
                returnItem.ReturnDate)
            .ToListAsync(cancellationToken);
    }
}