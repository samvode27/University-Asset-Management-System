using Microsoft.EntityFrameworkCore;
using UAMS.Application.Interfaces.Repositories;
using UAMS.Domain.Entities.Assets;
using UAMS.Domain.Enums;
using UAMS.Infrastructure.Persistence;

namespace UAMS.Infrastructure.Repositories;

public class AssetRepository
    : GenericRepository<Asset>, IAssetRepository
{
    public AssetRepository(UAMSDbContext context)
        : base(context)
    {
    }


    // ================================================================
    // Get Asset By Asset Number
    // ================================================================

    public virtual async Task<Asset?> GetByAssetNumberAsync(
        string assetNumber,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(assetNumber);

        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                asset => asset.AssetTag == assetNumber,
                cancellationToken);
    }


    // ================================================================
    // Get Asset By Serial Number
    // ================================================================

    public virtual async Task<Asset?> GetBySerialNumberAsync(
        string serialNumber,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(serialNumber);

        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                asset => asset.SerialNumber == serialNumber,
                cancellationToken);
    }


    // ================================================================
    // Get Assets By Status
    // ================================================================

    public virtual async Task<IReadOnlyList<Asset>>
        GetByStatusAsync(
            AssetStatus status,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(asset => asset.Status == status)
            .OrderBy(asset => asset.AssetTag)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Assets By Category
    // ================================================================

    public virtual async Task<IReadOnlyList<Asset>>
        GetByCategoryAsync(
            Guid assetCategoryId,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(asset => asset.AssetCategoryId == assetCategoryId)
            .OrderBy(asset => asset.AssetTag)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Assets By Department
    // ================================================================

    public virtual async Task<IReadOnlyList<Asset>>
        GetByDepartmentAsync(
            Guid departmentId,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(asset => asset.DepartmentId == departmentId)
            .OrderBy(asset => asset.AssetTag)
            .ToListAsync(cancellationToken);
    }


// ================================================================
// Get Assets Assigned To Employee
// ================================================================

public virtual async Task<IReadOnlyList<Asset>>
    GetAssignedToEmployeeAsync(
        Guid employeeId,
        CancellationToken cancellationToken = default)
{
    return await DbSet
        .AsNoTracking()
        .Where(asset =>
            asset.AssetAssignments.Any(
                assignment => assignment.EmployeeId == employeeId))
        .OrderBy(asset => asset.AssetTag)
        .ToListAsync(cancellationToken);
}
}