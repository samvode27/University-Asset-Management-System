using UAMS.Application.DTOs.AssetCategories.Requests;
using UAMS.Application.DTOs.AssetCategories.Responses;

namespace UAMS.Application.Interfaces.Services;

public interface IAssetCategoryService
{
    // ================================================================
    // Queries
    // ================================================================

    Task<AssetCategoryResponseDto> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<AssetCategoryDetailResponseDto> GetDetailsAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<AssetCategoryListResponseDto> GetAllAsync(
        AssetCategoryFilterRequestDto request,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Commands
    // ================================================================

    Task<AssetCategoryResponseDto> CreateAsync(
        CreateAssetCategoryRequestDto request,
        CancellationToken cancellationToken = default);

    Task<AssetCategoryResponseDto> UpdateAsync(
        Guid id,
        UpdateAssetCategoryRequestDto request,
        CancellationToken cancellationToken = default);

    Task<AssetCategoryResponseDto> ActivateAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<AssetCategoryResponseDto> DeactivateAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}

