using Microsoft.EntityFrameworkCore;
using UAMS.Application.Interfaces.Repositories;
using UAMS.Domain.Entities.AssetDisposals;
using UAMS.Domain.Enums;
using UAMS.Infrastructure.Persistence;

namespace UAMS.Infrastructure.Repositories;

public class AssetDisposalRepository
    : GenericRepository<AssetDisposal>, IAssetDisposalRepository
{
    public AssetDisposalRepository(UAMSDbContext context)
        : base(context)
    {
    }


    // ================================================================
    // Get Disposal By Disposal Number
    // ================================================================

    public virtual async Task<AssetDisposal?>
        GetByDisposalNumberAsync(
            string disposalNumber,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                disposal =>
                    disposal.DisposalNumber == disposalNumber,
                cancellationToken);
    }


    // ================================================================
    // Get Disposal Records By Asset
    // ================================================================

    public virtual async Task<IReadOnlyList<AssetDisposal>>
        GetByAssetIdAsync(
            Guid assetId,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(disposal =>
                disposal.AssetId == assetId)
            .OrderByDescending(disposal =>
                disposal.RequestedDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Disposal Records By Maintenance
    // ================================================================

    public virtual async Task<IReadOnlyList<AssetDisposal>>
        GetByMaintenanceIdAsync(
            Guid maintenanceId,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(disposal =>
                disposal.MaintenanceId == maintenanceId)
            .OrderByDescending(disposal =>
                disposal.RequestedDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Disposal Records Requested By User
    // ================================================================

    public virtual async Task<IReadOnlyList<AssetDisposal>>
        GetByRequestedByIdAsync(
            Guid requestedById,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(disposal =>
                disposal.RequestedById == requestedById)
            .OrderByDescending(disposal =>
                disposal.RequestedDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Disposal Records Approved By User
    // ================================================================

    public virtual async Task<IReadOnlyList<AssetDisposal>>
        GetByApprovedByIdAsync(
            Guid approvedById,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(disposal =>
                disposal.ApprovedById == approvedById)
            .OrderByDescending(disposal =>
                disposal.ApprovedDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Disposal Records Completed By User
    // ================================================================

    public virtual async Task<IReadOnlyList<AssetDisposal>>
        GetByCompletedByIdAsync(
            Guid completedById,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(disposal =>
                disposal.CompletedById == completedById)
            .OrderByDescending(disposal =>
                disposal.DisposalDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Disposal Records By Disposal Method
    // ================================================================

    public virtual async Task<IReadOnlyList<AssetDisposal>>
        GetByDisposalMethodAsync(
            DisposalMethod disposalMethod,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(disposal =>
                disposal.DisposalMethod == disposalMethod)
            .OrderByDescending(disposal =>
                disposal.RequestedDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Disposal Records By Status
    // ================================================================

    public virtual async Task<IReadOnlyList<AssetDisposal>>
        GetByStatusAsync(
            AssetDisposalStatus status,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(disposal =>
                disposal.Status == status)
            .OrderByDescending(disposal =>
                disposal.RequestedDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Pending Disposal Requests
    // ================================================================

    public virtual async Task<IReadOnlyList<AssetDisposal>>
        GetPendingAsync(
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(disposal =>
                disposal.Status == AssetDisposalStatus.Requested)
            .OrderBy(disposal =>
                disposal.RequestedDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Open Disposal Requests
    // ================================================================

    public virtual async Task<IReadOnlyList<AssetDisposal>>
        GetOpenAsync(
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(disposal =>
                disposal.Status == AssetDisposalStatus.Requested ||
                disposal.Status == AssetDisposalStatus.UnderReview ||
                disposal.Status == AssetDisposalStatus.Approved)
            .OrderBy(disposal =>
                disposal.RequestedDate)
            .ToListAsync(cancellationToken);
    }
}