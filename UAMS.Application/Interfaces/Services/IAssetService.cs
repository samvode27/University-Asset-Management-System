using UAMS.Application.DTOs.Assets.Requests;
using UAMS.Application.DTOs.Assets.Responses;

namespace UAMS.Application.Interfaces.Services;

public interface IAssetService
{
    // ================================================================
    // Asset Lookup
    // ================================================================

    Task<AssetResponseDto> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<AssetDetailResponseDto> GetDetailsAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<AssetListResponseDto> GetAllAsync(
        AssetFilterRequestDto request,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Asset Management
    // ================================================================

    Task<AssetResponseDto> CreateAsync(
        CreateAssetRequestDto request,
        CancellationToken cancellationToken = default);

    Task<AssetResponseDto> UpdateAsync(
        Guid id,
        UpdateAssetRequestDto request,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}