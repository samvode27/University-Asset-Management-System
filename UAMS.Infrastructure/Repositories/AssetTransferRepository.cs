using Microsoft.EntityFrameworkCore;
using UAMS.Application.Interfaces.Repositories;
using UAMS.Domain.Entities.AssetTransfers;
using UAMS.Domain.Enums;
using UAMS.Infrastructure.Persistence;

namespace UAMS.Infrastructure.Repositories;

public class AssetTransferRepository
    : GenericRepository<AssetTransfer>, IAssetTransferRepository
{
    public AssetTransferRepository(UAMSDbContext context)
        : base(context)
    {
    }


    // ================================================================
    // Get Transfer By Transfer Number
    // ================================================================

    public virtual async Task<AssetTransfer?>
        GetByTransferNumberAsync(
            string transferNumber,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                transfer => transfer.TransferNumber == transferNumber,
                cancellationToken);
    }


    // ================================================================
    // Get Transfers By Asset
    // ================================================================

    public virtual async Task<IReadOnlyList<AssetTransfer>>
        GetByAssetIdAsync(
            Guid assetId,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(transfer => transfer.AssetId == assetId)
            .OrderByDescending(transfer => transfer.RequestedDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Transfers By Asset Assignment
    // ================================================================

    public virtual async Task<IReadOnlyList<AssetTransfer>>
        GetByAssetAssignmentIdAsync(
            Guid assetAssignmentId,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(transfer =>
                transfer.AssetAssignmentId == assetAssignmentId)
            .OrderByDescending(transfer => transfer.RequestedDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Transfers Requested By User
    // ================================================================

    public virtual async Task<IReadOnlyList<AssetTransfer>>
        GetByRequestedByIdAsync(
            Guid requestedById,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(transfer =>
                transfer.RequestedById == requestedById)
            .OrderByDescending(transfer => transfer.RequestedDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Transfers From Employee
    // ================================================================

    public virtual async Task<IReadOnlyList<AssetTransfer>>
        GetByFromEmployeeIdAsync(
            Guid fromEmployeeId,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(transfer =>
                transfer.FromEmployeeId == fromEmployeeId)
            .OrderByDescending(transfer => transfer.RequestedDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Transfers To Employee
    // ================================================================

    public virtual async Task<IReadOnlyList<AssetTransfer>>
        GetByToEmployeeIdAsync(
            Guid toEmployeeId,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(transfer =>
                transfer.ToEmployeeId == toEmployeeId)
            .OrderByDescending(transfer => transfer.RequestedDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Transfers From Department
    // ================================================================

    public virtual async Task<IReadOnlyList<AssetTransfer>>
        GetByFromDepartmentIdAsync(
            Guid fromDepartmentId,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(transfer =>
                transfer.FromDepartmentId == fromDepartmentId)
            .OrderByDescending(transfer => transfer.RequestedDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Transfers To Department
    // ================================================================

    public virtual async Task<IReadOnlyList<AssetTransfer>>
        GetByToDepartmentIdAsync(
            Guid toDepartmentId,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(transfer =>
                transfer.ToDepartmentId == toDepartmentId)
            .OrderByDescending(transfer => transfer.RequestedDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Transfers By Status
    // ================================================================

    public virtual async Task<IReadOnlyList<AssetTransfer>>
        GetByStatusAsync(
            AssetTransferStatus status,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(transfer => transfer.Status == status)
            .OrderByDescending(transfer => transfer.RequestedDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Pending Transfers
    // ================================================================

    public virtual async Task<IReadOnlyList<AssetTransfer>>
        GetPendingAsync(
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(transfer =>
                transfer.Status == AssetTransferStatus.PendingApproval)
            .OrderBy(transfer => transfer.RequestedDate)
            .ToListAsync(cancellationToken);
    }
}