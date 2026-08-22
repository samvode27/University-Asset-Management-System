using UAMS.Application.DTOs.Roles.Requests;
using UAMS.Application.DTOs.Roles.Responses;

namespace UAMS.Application.Interfaces.Services;

public interface IRoleService
{
    Task<RoleResponseDto> CreateAsync(
        CreateRoleRequestDto request,
        CancellationToken cancellationToken = default);

    Task<RoleResponseDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<RoleDetailResponseDto?> GetDetailsAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<RoleListResponseDto> GetAllAsync(
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default);

    Task<RoleResponseDto> UpdateAsync(
        Guid id,
        UpdateRoleRequestDto request,
        CancellationToken cancellationToken = default);

    Task AssignPermissionsAsync(
        Guid id,
        AssignPermissionsRequestDto request,
        CancellationToken cancellationToken = default);

    Task RemovePermissionsAsync(
        Guid id,
        RemovePermissionsRequestDto request,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<RoleResponseDto>> GetActiveRolesAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<RoleResponseDto>> GetSystemRolesAsync(
        CancellationToken cancellationToken = default);
}