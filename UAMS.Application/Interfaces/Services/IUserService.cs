using UAMS.Application.DTOs.Users.Requests;
using UAMS.Application.DTOs.Users.Responses;

namespace UAMS.Application.Interfaces.Services;

public interface IUserService
{
    Task<UserDetailResponseDto> CreateUserAsync(
        CreateUserRequestDto request,
        CancellationToken cancellationToken = default);

    Task<UserDetailResponseDto> GetUserByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<UserListResponseDto> GetUsersAsync(
        UserFilterRequestDto request,
        CancellationToken cancellationToken = default);

    Task<UserDetailResponseDto> UpdateUserAsync(
        Guid id,
        UpdateUserRequestDto request,
        CancellationToken cancellationToken = default);

    Task DeleteUserAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task ActivateUserAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task DeactivateUserAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task ResetUserPasswordAsync(
        Guid id,
        ResetUserPasswordRequestDto request,
        CancellationToken cancellationToken = default);

    Task AssignRoleAsync(
        Guid id,
        AssignRoleRequestDto request,
        CancellationToken cancellationToken = default);

    Task ChangeDepartmentAsync(
        Guid id,
        ChangeDepartmentRequestDto request,
        CancellationToken cancellationToken = default);
}