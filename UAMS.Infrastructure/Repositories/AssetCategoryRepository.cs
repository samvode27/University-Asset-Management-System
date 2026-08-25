using Microsoft.EntityFrameworkCore;
using UAMS.Application.Interfaces.Repositories;
using UAMS.Domain.Entities.AssetCategories;
using UAMS.Infrastructure.Persistence;

namespace UAMS.Infrastructure.Repositories;

public class AssetCategoryRepository
    : GenericRepository<AssetCategory>, IAssetCategoryRepository
{
    public AssetCategoryRepository(UAMSDbContext context)
        : base(context)
    {
    }


    // ================================================================
    // Get Asset Category By Name
    // ================================================================

    public virtual async Task<AssetCategory?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                category => category.Name == name,
                cancellationToken);
    }


    // ================================================================
    // Get Asset Category By Id With Details
    // ================================================================

    public virtual async Task<AssetCategory?> GetByIdWithDetailsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Asset category ID is required.",
                nameof(id));
        }

        return await DbSet
            .Include(category => category.Assets)
            .AsNoTracking()
            .FirstOrDefaultAsync(
                category => category.Id == id,
                cancellationToken);
    }



    // ================================================================
    // Get Active Asset Categories
    // ================================================================

    public virtual async Task<IReadOnlyList<AssetCategory>>
        GetActiveCategoriesAsync(
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(category => category.IsActive)
            .OrderBy(category => category.Name)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Inactive Asset Categories
    // ================================================================

    public virtual async Task<IReadOnlyList<AssetCategory>>
        GetInactiveCategoriesAsync(
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(category => !category.IsActive)
            .OrderBy(category => category.Name)
            .ToListAsync(cancellationToken);
    }
}