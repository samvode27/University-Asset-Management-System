using UAMS.Domain.Entities.Assets;
using UAMS.Domain.Enums;

namespace UAMS.Application.Interfaces.Repositories;

public interface IAssetRepository : IRepository<Asset>
{
    // ================================================================
    // Asset Lookup
    // ================================================================

    Task<Asset?> GetByAssetNumberAsync(
        string assetNumber,
        CancellationToken cancellationToken = default);

    Task<Asset?> GetBySerialNumberAsync(
        string serialNumber,
        CancellationToken cancellationToken = default);

    Task<Asset?> GetByIdWithDetailsAsync(
        Guid id,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Asset Status
    // ================================================================

    Task<IReadOnlyList<Asset>> GetByStatusAsync(
        AssetStatus status,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Asset Category
    // ================================================================

    Task<IReadOnlyList<Asset>> GetByCategoryAsync(
        Guid assetCategoryId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Department
    // ================================================================

    Task<IReadOnlyList<Asset>> GetByDepartmentAsync(
        Guid departmentId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Employee Assignment
    // ================================================================

    Task<IReadOnlyList<Asset>> GetAssignedToEmployeeAsync(
        Guid employeeId,
        CancellationToken cancellationToken = default);
}