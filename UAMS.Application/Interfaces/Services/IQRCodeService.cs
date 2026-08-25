using UAMS.Application.DTOs.QRCode.Requests;
using UAMS.Application.DTOs.QRCode.Responses;

namespace UAMS.Application.Interfaces.Services;

public interface IQRCodeService
{
    // ================================================================
    // QR Code Lookup
    // ================================================================

    Task<QRCodeResponseDto> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<QRCodeDetailResponseDto> GetDetailsAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<QRCodeResponseDto> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Asset-Based Lookup
    // ================================================================

    Task<QRCodeResponseDto> GetByAssetIdAsync(
        Guid assetId,
        CancellationToken cancellationToken = default);

    Task<QRCodeResponseDto> GetActiveByAssetIdAsync(
        Guid assetId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // QR Code List
    // ================================================================

    Task<QRCodeListResponseDto> GetAllAsync(
        QRCodeFilterRequestDto request,
        CancellationToken cancellationToken = default);


    // ================================================================
    // QR Code Generation
    // ================================================================

    Task<QRCodeResponseDto> GenerateAsync(
        GenerateQRCodeRequestDto request,
        CancellationToken cancellationToken = default);


    // ================================================================
    // QR Code Update
    // ================================================================

    Task<QRCodeResponseDto> UpdateAsync(
        Guid id,
        UpdateQRCodeRequestDto request,
        CancellationToken cancellationToken = default);


    // ================================================================
    // QR Code Delete
    // ================================================================

    Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}