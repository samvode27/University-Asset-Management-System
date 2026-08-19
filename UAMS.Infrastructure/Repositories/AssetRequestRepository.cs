using Microsoft.EntityFrameworkCore;
using UAMS.Application.Interfaces.Repositories;
using UAMS.Domain.Entities.AssetRequests;
using UAMS.Domain.Enums;
using UAMS.Infrastructure.Persistence;

namespace UAMS.Infrastructure.Repositories;

public class AssetRequestRepository
    : GenericRepository<AssetRequest>, IAssetRequestRepository
{
    public AssetRequestRepository(UAMSDbContext context)
        : base(context)
    {
    }

    public virtual async Task<AssetRequest?> GetByRequestNumberAsync(
        string requestNumber,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(requestNumber);

        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                request => request.RequestNumber == requestNumber,
                cancellationToken);
    }

    public virtual async Task<IReadOnlyList<AssetRequest>>
        GetByRequesterIdAsync(
            Guid requesterId,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(request => request.RequesterId == requesterId)
            .OrderByDescending(request => request.RequestedDate)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<IReadOnlyList<AssetRequest>>
        GetByAssetIdAsync(
            Guid assetId,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(request => request.AssetId == assetId)
            .OrderByDescending(request => request.RequestedDate)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<IReadOnlyList<AssetRequest>>
        GetByDepartmentIdAsync(
            Guid departmentId,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(request => request.DepartmentId == departmentId)
            .OrderByDescending(request => request.RequestedDate)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<IReadOnlyList<AssetRequest>>
        GetByStatusAsync(
            AssetRequestStatus status,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(request => request.Status == status)
            .OrderBy(request => request.RequestedDate)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<IReadOnlyList<AssetRequest>>
        GetByRequesterAndStatusAsync(
            Guid requesterId,
            AssetRequestStatus status,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(request =>
                request.RequesterId == requesterId &&
                request.Status == status)
            .OrderByDescending(request => request.RequestedDate)
            .ToListAsync(cancellationToken);
    }
}