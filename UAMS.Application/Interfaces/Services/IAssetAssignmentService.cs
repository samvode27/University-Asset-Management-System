using UAMS.Application.DTOs.AssetAssignments.Requests;
using UAMS.Application.DTOs.AssetAssignments.Responses;
using UAMS.Domain.Enums;

namespace UAMS.Application.Interfaces.Services;

public interface IAssetAssignmentService
{
    // ================================================================
    // Create
    // ================================================================

    Task<AssetAssignmentResponseDto> CreateAsync(
        CreateAssetAssignmentRequestDto request,
        Guid assignedById,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get By Id
    // ================================================================

    Task<AssetAssignmentDetailResponseDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get By Asset
    // ================================================================

    Task<IReadOnlyList<AssetAssignmentResponseDto>>
        GetByAssetIdAsync(
            Guid assetId,
            CancellationToken cancellationToken = default);


    // ================================================================
    // Get By Employee
    // ================================================================

    Task<IReadOnlyList<AssetAssignmentResponseDto>>
        GetByEmployeeIdAsync(
            Guid employeeId,
            CancellationToken cancellationToken = default);


    // ================================================================
    // Get By Asset Request
    // ================================================================

    Task<AssetAssignmentResponseDto?> GetByAssetRequestIdAsync(
        Guid assetRequestId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Active By Asset
    // ================================================================

    Task<AssetAssignmentResponseDto?> GetActiveByAssetIdAsync(
        Guid assetId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Active By Employee
    // ================================================================

    Task<IReadOnlyList<AssetAssignmentResponseDto>>
        GetActiveByEmployeeIdAsync(
            Guid employeeId,
            CancellationToken cancellationToken = default);


    // ================================================================
    // Get By Status
    // ================================================================

    Task<IReadOnlyList<AssetAssignmentResponseDto>>
        GetByStatusAsync(
            AssetAssignmentStatus status,
            CancellationToken cancellationToken = default);


    // ================================================================
    // Update
    // ================================================================

    Task<AssetAssignmentResponseDto> UpdateAsync(
        Guid id,
        UpdateAssetAssignmentRequestDto request,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Complete / Return
    // ================================================================

    Task<AssetAssignmentResponseDto> CompleteAsync(
        Guid id,
        CompleteAssetAssignmentRequestDto request,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Cancel
    // ================================================================

    Task<AssetAssignmentResponseDto> CancelAsync(
        Guid id,
        string? reason,
        CancellationToken cancellationToken = default);
}