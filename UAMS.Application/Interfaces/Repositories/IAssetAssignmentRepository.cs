using UAMS.Domain.Entities.AssetAssignments;

namespace UAMS.Application.Interfaces.Repositories;

public interface IAssetAssignmentRepository
    : IRepository<AssetAssignment>
{
    // ================================================================
    // Get Assignments By Asset
    // ================================================================

    Task<IReadOnlyList<AssetAssignment>> GetByAssetIdAsync(
        Guid assetId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Assignments By Employee
    // ================================================================

    Task<IReadOnlyList<AssetAssignment>> GetByEmployeeIdAsync(
        Guid employeeId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Assignment By Asset Request
    // ================================================================

    Task<AssetAssignment?> GetByAssetRequestIdAsync(
        Guid assetRequestId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Active Assignment By Asset
    // ================================================================

    Task<AssetAssignment?> GetActiveByAssetIdAsync(
        Guid assetId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Active Assignment By Employee
    // ================================================================

    Task<IReadOnlyList<AssetAssignment>> GetActiveByEmployeeIdAsync(
        Guid employeeId,
        CancellationToken cancellationToken = default);
}