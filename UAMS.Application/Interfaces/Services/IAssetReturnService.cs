using UAMS.Application.DTOs.AssetReturns.Requests;
using UAMS.Application.DTOs.AssetReturns.Responses;
using UAMS.Domain.Enums;

namespace UAMS.Application.Interfaces.Services;

public interface IAssetReturnService
{
    // ================================================================
    // Create
    // ================================================================

    Task<AssetReturnResponseDto> CreateAsync(
        CreateAssetReturnRequestDto request,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get By Id
    // ================================================================

    Task<AssetReturnDetailResponseDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get By Return Number
    // ================================================================

    Task<AssetReturnResponseDto?> GetByReturnNumberAsync(
        string returnNumber,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get By Asset
    // ================================================================

    Task<IReadOnlyList<AssetReturnResponseDto>>
        GetByAssetIdAsync(
            Guid assetId,
            CancellationToken cancellationToken = default);


    // ================================================================
    // Get By Asset Assignment
    // ================================================================

    Task<IReadOnlyList<AssetReturnResponseDto>>
        GetByAssetAssignmentIdAsync(
            Guid assetAssignmentId,
            CancellationToken cancellationToken = default);


    // ================================================================
    // Get By Employee
    // ================================================================

    Task<IReadOnlyList<AssetReturnResponseDto>>
        GetByEmployeeIdAsync(
            Guid employeeId,
            CancellationToken cancellationToken = default);


    // ================================================================
    // Get By Received By
    // ================================================================

    Task<IReadOnlyList<AssetReturnResponseDto>>
        GetByReceivedByIdAsync(
            Guid receivedById,
            CancellationToken cancellationToken = default);


    // ================================================================
    // Get By Inspector
    // ================================================================

    Task<IReadOnlyList<AssetReturnResponseDto>>
        GetByInspectedByIdAsync(
            Guid inspectedById,
            CancellationToken cancellationToken = default);


    // ================================================================
    // Get By Status
    // ================================================================

    Task<IReadOnlyList<AssetReturnResponseDto>>
        GetByStatusAsync(
            AssetReturnStatus status,
            CancellationToken cancellationToken = default);


    // ================================================================
    // Get Pending Inspection
    // ================================================================

    Task<IReadOnlyList<AssetReturnResponseDto>>
        GetPendingInspectionAsync(
            CancellationToken cancellationToken = default);


    // ================================================================
    // Get Returns With Damage
    // ================================================================

    Task<IReadOnlyList<AssetReturnResponseDto>>
        GetWithDamageAsync(
            CancellationToken cancellationToken = default);


// ================================================================
// Filter / Search / Pagination
// ================================================================

Task<AssetReturnListResponseDto> FilterAsync(
    AssetReturnFilterRequestDto request,
    CancellationToken cancellationToken = default);

    // ================================================================
    // Update
    // ================================================================

    Task<AssetReturnResponseDto> UpdateAsync(
        Guid id,
        UpdateAssetReturnRequestDto request,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Inspect
    // ================================================================

    Task<AssetReturnResponseDto> InspectAsync(
        Guid id,
        InspectAssetReturnRequestDto request,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Complete
    // ================================================================

    Task<AssetReturnResponseDto> CompleteAsync(
        Guid id,
        CompleteAssetReturnRequestDto request,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Cancel
    // ================================================================

    Task<AssetReturnResponseDto> CancelAsync(
        Guid id,
        CancelAssetReturnRequestDto request,
        CancellationToken cancellationToken = default);
}