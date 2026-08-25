using UAMS.Application.DTOs.Barcode.Requests;
using UAMS.Application.DTOs.Barcode.Responses;

namespace UAMS.Application.Interfaces.Services;

public interface IBarcodeService
{
    // ================================================================
    // Barcode Lookup
    // ================================================================

    Task<BarcodeResponseDto> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<BarcodeDetailResponseDto> GetDetailsAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<BarcodeResponseDto> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Asset-Based Lookup
    // ================================================================

    Task<BarcodeResponseDto> GetByAssetIdAsync(
        Guid assetId,
        CancellationToken cancellationToken = default);

    Task<BarcodeResponseDto> GetActiveByAssetIdAsync(
        Guid assetId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Barcode List
    // ================================================================

    Task<BarcodeListResponseDto> GetAllAsync(
        BarcodeFilterRequestDto request,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Barcode Generation
    // ================================================================

    Task<BarcodeResponseDto> GenerateAsync(
        GenerateBarcodeRequestDto request,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Barcode Update
    // ================================================================

    Task<BarcodeResponseDto> UpdateAsync(
        Guid id,
        UpdateBarcodeRequestDto request,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Barcode Delete
    // ================================================================

    Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}