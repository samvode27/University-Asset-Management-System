using UAMS.Application.DTOs.Suppliers.Requests;
using UAMS.Application.DTOs.Suppliers.Responses;

namespace UAMS.Application.Interfaces.Services;

public interface ISupplierService
{
    Task<SupplierResponseDto> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<SupplierDetailResponseDto> GetDetailsAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<SupplierListResponseDto> GetAllAsync(
        SupplierFilterRequestDto request,
        CancellationToken cancellationToken = default);

    Task<SupplierResponseDto> CreateAsync(
        CreateSupplierRequestDto request,
        CancellationToken cancellationToken = default);

    Task<SupplierResponseDto> UpdateAsync(
        Guid id,
        UpdateSupplierRequestDto request,
        CancellationToken cancellationToken = default);

    Task ActivateAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task DeactivateAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}

