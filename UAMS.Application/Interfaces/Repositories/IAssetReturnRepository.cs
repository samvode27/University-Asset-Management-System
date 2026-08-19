using UAMS.Domain.Entities.AssetReturns;
using UAMS.Domain.Enums;

namespace UAMS.Application.Interfaces.Repositories;

public interface IAssetReturnRepository
    : IRepository<AssetReturn>
{
    // ================================================================
    // Get Return By Return Number
    // ================================================================

    Task<AssetReturn?> GetByReturnNumberAsync(
        string returnNumber,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Returns By Asset
    // ================================================================

    Task<IReadOnlyList<AssetReturn>> GetByAssetIdAsync(
        Guid assetId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Returns By Asset Assignment
    // ================================================================

    Task<IReadOnlyList<AssetReturn>> GetByAssetAssignmentIdAsync(
        Guid assetAssignmentId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Returns By Employee
    // ================================================================

    Task<IReadOnlyList<AssetReturn>> GetByEmployeeIdAsync(
        Guid employeeId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Returns Received By User
    // ================================================================

    Task<IReadOnlyList<AssetReturn>> GetByReceivedByIdAsync(
        Guid receivedById,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Returns Inspected By User
    // ================================================================

    Task<IReadOnlyList<AssetReturn>> GetByInspectedByIdAsync(
        Guid inspectedById,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Returns By Status
    // ================================================================

    Task<IReadOnlyList<AssetReturn>> GetByStatusAsync(
        AssetReturnStatus status,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Returns Pending Inspection
    // ================================================================

    Task<IReadOnlyList<AssetReturn>> GetPendingInspectionAsync(
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Returns With Damage
    // ================================================================

    Task<IReadOnlyList<AssetReturn>> GetWithDamageAsync(
        CancellationToken cancellationToken = default);
}