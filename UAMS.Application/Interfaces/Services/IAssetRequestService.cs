using UAMS.Application.DTOs.AssetRequests.Requests;
using UAMS.Application.DTOs.AssetRequests.Responses;
using UAMS.Domain.Enums;

namespace UAMS.Application.Interfaces.Services;

public interface IAssetRequestService
{
    // ================================================================
    // Asset Request Lookup
    // ================================================================

    Task<AssetRequestResponseDto> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<AssetRequestDetailResponseDto> GetDetailsAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<AssetRequestResponseDto> GetByRequestNumberAsync(
        string requestNumber,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Requester-Based Lookup
    // ================================================================

    Task<IReadOnlyList<AssetRequestResponseDto>> GetByRequesterIdAsync(
        Guid requesterId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Asset-Based Lookup
    // ================================================================

    Task<IReadOnlyList<AssetRequestResponseDto>> GetByAssetIdAsync(
        Guid assetId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Department-Based Lookup
    // ================================================================

    Task<IReadOnlyList<AssetRequestResponseDto>> GetByDepartmentIdAsync(
        Guid departmentId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Status-Based Lookup
    // ================================================================

    Task<IReadOnlyList<AssetRequestResponseDto>> GetByStatusAsync(
        AssetRequestStatus status,
        CancellationToken cancellationToken = default);


    Task<IReadOnlyList<AssetRequestResponseDto>>
        GetByRequesterAndStatusAsync(
            Guid requesterId,
            AssetRequestStatus status,
            CancellationToken cancellationToken = default);


    // ================================================================
    // Asset Request List
    // ================================================================

    Task<AssetRequestListResponseDto> GetAllAsync(
        AssetRequestFilterRequestDto request,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Create
    // ================================================================

    Task<AssetRequestResponseDto> CreateAsync(
        CreateAssetRequestDto request,
        Guid requesterId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Update
    // ================================================================

    Task<AssetRequestResponseDto> UpdateAsync(
        Guid id,
        UpdateAssetRequestDto request,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Department Head Approval
    // ================================================================

    Task<AssetRequestApprovalResponseDto>
        ReviewByDepartmentHeadAsync(
            Guid id,
            DepartmentHeadReviewRequestDto request,
            Guid departmentHeadId,
            CancellationToken cancellationToken = default);


    // ================================================================
    // Asset Manager Approval
    // ================================================================

    Task<AssetRequestApprovalResponseDto>
        ReviewByAssetManagerAsync(
            Guid id,
            AssetManagerReviewRequestDto request,
            Guid assetManagerId,
            CancellationToken cancellationToken = default);


    // ================================================================
    // Cancellation
    // ================================================================

    Task CancelAsync(
        Guid id,
        CancelAssetRequestDto request,
        CancellationToken cancellationToken = default);
}