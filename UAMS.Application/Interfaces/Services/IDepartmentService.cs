using UAMS.Application.DTOs.Departments.Requests;
using UAMS.Application.DTOs.Departments.Responses;

namespace UAMS.Application.Interfaces.Services;

public interface IDepartmentService
{
    Task<DepartmentResponseDto> CreateAsync(
        CreateDepartmentRequestDto request,
        CancellationToken cancellationToken = default);

    Task<DepartmentResponseDto> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<DepartmentDetailResponseDto> GetDetailsAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<DepartmentListResponseDto> GetAllAsync(
        DepartmentFilterRequestDto request,
        CancellationToken cancellationToken = default);

    Task<DepartmentResponseDto> UpdateAsync(
        Guid id,
        UpdateDepartmentRequestDto request,
        CancellationToken cancellationToken = default);

    Task<DepartmentResponseDto> ActivateAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<DepartmentResponseDto> DeactivateAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<DepartmentResponseDto>> GetActiveDepartmentsAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<DepartmentResponseDto>> GetInactiveDepartmentsAsync(
        CancellationToken cancellationToken = default);
}