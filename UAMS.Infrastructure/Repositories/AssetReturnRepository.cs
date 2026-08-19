using Microsoft.EntityFrameworkCore;
using UAMS.Application.Interfaces.Repositories;
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