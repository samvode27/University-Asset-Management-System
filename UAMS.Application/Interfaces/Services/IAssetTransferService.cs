using UAMS.Application.DTOs.AssetTransfers.Requests;
using UAMS.Application.DTOs.AssetTransfers.Responses;
using UAMS.Domain.Enums;

namespace UAMS.Application.Interfaces.Services;

public interface IAssetTransferService
{
    // ================================================================
    // Create
    // ================================================================

    Task<AssetTransferResponseDto> CreateAsync(
        CreateAssetTransferRequestDto request,
        Guid requestedById,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get By Id
    // ================================================================

    Task<AssetTransferDetailResponseDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get By Transfer Number
    // ================================================================

    Task<AssetTransferResponseDto?> GetByTransferNumberAsync(
        string transferNumber,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get By Asset
    // ================================================================

    Task<IReadOnlyList<AssetTransferResponseDto>>
        GetByAssetIdAsync(
            Guid assetId,
            CancellationToken cancellationToken = default);


    // ================================================================
    // Get By Asset Assignment
    // ================================================================

    Task<IReadOnlyList<AssetTransferResponseDto>>
        GetByAssetAssignmentIdAsync(
            Guid assetAssignmentId,
            CancellationToken cancellationToken = default);


    // ================================================================
    // Get By Requested By
    // ================================================================

    Task<IReadOnlyList<AssetTransferResponseDto>>
        GetByRequestedByIdAsync(
            Guid requestedById,
            CancellationToken cancellationToken = default);


    // ================================================================
    // Get By From Employee
    // ================================================================

    Task<IReadOnlyList<AssetTransferResponseDto>>
        GetByFromEmployeeIdAsync(
            Guid fromEmployeeId,
            CancellationToken cancellationToken = default);


    // ================================================================
    // Get By To Employee
    // ================================================================

    Task<IReadOnlyList<AssetTransferResponseDto>>
        GetByToEmployeeIdAsync(
            Guid toEmployeeId,
            CancellationToken cancellationToken = default);


    // ================================================================
    // Get By From Department
    // ================================================================

    Task<IReadOnlyList<AssetTransferResponseDto>>
        GetByFromDepartmentIdAsync(
            Guid fromDepartmentId,
            CancellationToken cancellationToken = default);


    // ================================================================
    // Get By To Department
    // ================================================================

    Task<IReadOnlyList<AssetTransferResponseDto>>
        GetByToDepartmentIdAsync(
            Guid toDepartmentId,
            CancellationToken cancellationToken = default);


    // ================================================================
    // Get By Status
    // ================================================================

    Task<IReadOnlyList<AssetTransferResponseDto>>
        GetByStatusAsync(
            AssetTransferStatus status,
            CancellationToken cancellationToken = default);


    // ================================================================
    // Get Pending
    // ================================================================

    Task<IReadOnlyList<AssetTransferResponseDto>>
        GetPendingAsync(
            CancellationToken cancellationToken = default);


    // ================================================================
    // Update
    // ================================================================

    Task<AssetTransferResponseDto> UpdateAsync(
        Guid id,
        UpdateAssetTransferRequestDto request,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Approve
    // ================================================================

    Task<AssetTransferResponseDto> ApproveAsync(
        Guid id,
        ApproveAssetTransferRequestDto request,
        Guid approvedById,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Reject
    // ================================================================

    Task<AssetTransferResponseDto> RejectAsync(
        Guid id,
        RejectAssetTransferRequestDto request,
        Guid approvedById,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Complete
    // ================================================================

    Task<AssetTransferResponseDto> CompleteAsync(
        Guid id,
        CompleteAssetTransferRequestDto request,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Cancel
    // ================================================================

    Task<AssetTransferResponseDto> CancelAsync(
        Guid id,
        string? reason,
        CancellationToken cancellationToken = default);
}