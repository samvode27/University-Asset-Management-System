using UAMS.Domain.Entities.AssetDisposals;
using UAMS.Domain.Enums;

namespace UAMS.Application.Interfaces.Repositories;

public interface IAssetDisposalRepository
    : IRepository<AssetDisposal>
{
    // ================================================================
    // Get Disposal By Disposal Number
    // ================================================================

    Task<AssetDisposal?> GetByDisposalNumberAsync(
        string disposalNumber,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Disposal Records By Asset
    // ================================================================

    Task<IReadOnlyList<AssetDisposal>> GetByAssetIdAsync(
        Guid assetId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Disposal Records By Maintenance
    // ================================================================

    Task<IReadOnlyList<AssetDisposal>> GetByMaintenanceIdAsync(
        Guid maintenanceId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Disposal Records Requested By User
    // ================================================================

    Task<IReadOnlyList<AssetDisposal>> GetByRequestedByIdAsync(
        Guid requestedById,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Disposal Records Approved By User
    // ================================================================

    Task<IReadOnlyList<AssetDisposal>> GetByApprovedByIdAsync(
        Guid approvedById,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Disposal Records Completed By User
    // ================================================================

    Task<IReadOnlyList<AssetDisposal>> GetByCompletedByIdAsync(
        Guid completedById,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Disposal Records By Disposal Method
    // ================================================================

    Task<IReadOnlyList<AssetDisposal>> GetByDisposalMethodAsync(
        DisposalMethod disposalMethod,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Disposal Records By Status
    // ================================================================

    Task<IReadOnlyList<AssetDisposal>> GetByStatusAsync(
        AssetDisposalStatus status,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Pending Disposal Requests
    // ================================================================

    Task<IReadOnlyList<AssetDisposal>> GetPendingAsync(
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Open Disposal Requests
    // ================================================================

    Task<IReadOnlyList<AssetDisposal>> GetOpenAsync(
        CancellationToken cancellationToken = default);
}