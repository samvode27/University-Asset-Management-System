using UAMS.Application.DTOs.AssetDisposals.Requests;
using UAMS.Application.DTOs.AssetDisposals.Responses;

namespace UAMS.Application.Interfaces.Services;

public interface IAssetDisposalService
{
    Task<AssetDisposalResponseDto> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<AssetDisposalDetailResponseDto> GetDetailsAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<AssetDisposalResponseDto?> GetByDisposalNumberAsync(
        string disposalNumber,
        CancellationToken cancellationToken = default);

    Task<AssetDisposalListResponseDto> GetAllAsync(
        AssetDisposalFilterRequestDto request,
        CancellationToken cancellationToken = default);

    Task<AssetDisposalResponseDto> CreateAsync(
        CreateAssetDisposalRequestDto request,
        Guid requestedById,
        CancellationToken cancellationToken = default);

    Task<AssetDisposalResponseDto> UpdateAsync(
        Guid id,
        UpdateAssetDisposalRequestDto request,
        CancellationToken cancellationToken = default);

    Task<AssetDisposalResponseDto> ApproveAsync(
        Guid id,
        ApproveAssetDisposalRequestDto request,
        Guid approvedById,
        CancellationToken cancellationToken = default);

    Task<AssetDisposalResponseDto> RejectAsync(
        Guid id,
        RejectAssetDisposalRequestDto request,
        CancellationToken cancellationToken = default);

    Task<AssetDisposalResponseDto> CompleteAsync(
        Guid id,
        CompleteAssetDisposalRequestDto request,
        Guid completedById,
        CancellationToken cancellationToken = default);

    Task StartReviewAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}