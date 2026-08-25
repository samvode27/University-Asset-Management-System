using UAMS.Application.DTOs.Purchases.Requests;
using UAMS.Application.DTOs.Purchases.Responses;

namespace UAMS.Application.Interfaces.Services;

public interface IPurchaseService
{
    // ================================================================
    // Query
    // ================================================================

    Task<PurchaseResponseDto> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<PurchaseDetailResponseDto> GetDetailsAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<PurchaseListResponseDto> GetAllAsync(
        PurchaseFilterRequestDto request,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Command
    // ================================================================

    Task<PurchaseResponseDto> CreateAsync(
        CreatePurchaseRequestDto request,
        CancellationToken cancellationToken = default);

    Task<PurchaseResponseDto> UpdateAsync(
        Guid id,
        UpdatePurchaseRequestDto request,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}