using UAMS.Domain.Entities.AssetTransfers;
using UAMS.Domain.Enums;

namespace UAMS.Application.Interfaces.Repositories;

public interface IAssetTransferRepository
    : IRepository<AssetTransfer>
{
    // ================================================================
    // Get Transfer By Transfer Number
    // ================================================================

    Task<AssetTransfer?> GetByTransferNumberAsync(
        string transferNumber,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Transfers By Asset
    // ================================================================

    Task<IReadOnlyList<AssetTransfer>> GetByAssetIdAsync(
        Guid assetId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Transfers By Asset Assignment
    // ================================================================

    Task<IReadOnlyList<AssetTransfer>> GetByAssetAssignmentIdAsync(
        Guid assetAssignmentId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Transfers Requested By User
    // ================================================================

    Task<IReadOnlyList<AssetTransfer>> GetByRequestedByIdAsync(
        Guid requestedById,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Transfers From Employee
    // ================================================================

    Task<IReadOnlyList<AssetTransfer>> GetByFromEmployeeIdAsync(
        Guid fromEmployeeId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Transfers To Employee
    // ================================================================

    Task<IReadOnlyList<AssetTransfer>> GetByToEmployeeIdAsync(
        Guid toEmployeeId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Transfers From Department
    // ================================================================

    Task<IReadOnlyList<AssetTransfer>> GetByFromDepartmentIdAsync(
        Guid fromDepartmentId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Transfers To Department
    // ================================================================

    Task<IReadOnlyList<AssetTransfer>> GetByToDepartmentIdAsync(
        Guid toDepartmentId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Transfers By Status
    // ================================================================

    Task<IReadOnlyList<AssetTransfer>> GetByStatusAsync(
        AssetTransferStatus status,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Pending Transfers
    // ================================================================

    Task<IReadOnlyList<AssetTransfer>> GetPendingAsync(
        CancellationToken cancellationToken = default);
}