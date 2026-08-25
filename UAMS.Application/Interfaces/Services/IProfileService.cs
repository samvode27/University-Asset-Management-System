using UAMS.Application.DTOs.Profile.Requests;
using UAMS.Application.DTOs.Profile.Responses;

namespace UAMS.Application.Interfaces.Services;

public interface IProfileService
{
    // ============================================================
    // Get Current Profile
    // ============================================================

    Task<ProfileDetailResponseDto?> GetProfileAsync(
        Guid userId,
        CancellationToken cancellationToken = default);


    // ============================================================
    // Get Profile Summary
    // ============================================================

    Task<ProfileSummaryResponseDto?> GetProfileSummaryAsync(
        Guid userId,
        CancellationToken cancellationToken = default);


    // ============================================================
    // Update Profile
    // ============================================================

    Task<ProfileResponseDto?> UpdateProfileAsync(
        Guid userId,
        UpdateProfileRequestDto request,
        CancellationToken cancellationToken = default);


    // ============================================================
    // Update Profile Picture
    // ============================================================

    Task<ProfilePictureResponseDto?> UpdateProfilePictureAsync(
        Guid userId,
        UpdateProfilePictureRequestDto request,
        CancellationToken cancellationToken = default);


    // ============================================================
    // Update Preferences
    // ============================================================

    Task<ProfilePreferencesResponseDto?> UpdatePreferencesAsync(
        Guid userId,
        UpdateProfilePreferencesRequestDto request,
        CancellationToken cancellationToken = default);
}