using UAMS.Domain.Entities.AssetCategories;

namespace UAMS.Application.Interfaces.Repositories;

public interface IAssetCategoryRepository
    : IRepository<AssetCategory>
{
    // ================================================================
    // Category Lookup
    // ================================================================

    Task<AssetCategory?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Category Status
    // ================================================================

    Task<IReadOnlyList<AssetCategory>> GetActiveCategoriesAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AssetCategory>> GetInactiveCategoriesAsync(
        CancellationToken cancellationToken = default);
}