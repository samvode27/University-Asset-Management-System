using UAMS.Application.DTOs.Profile.Requests;
using UAMS.Application.DTOs.Profile.Responses;
using UAMS.Application.Interfaces.Persistence;
using UAMS.Application.Interfaces.Repositories;
using UAMS.Application.Interfaces.Services;
using UAMS.Domain.Entities.FileAttachments;
using UAMS.Domain.Entities.Users;
using UAMS.Domain.Enums;

namespace UAMS.Application.Services;

public class ProfileService : IProfileService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProfileService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    // ============================================================
    // Get Current Profile
    // ============================================================

    public async Task<ProfileDetailResponseDto?>
        GetProfileAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
    {
        var user =
            await _unitOfWork.Users.GetWithProfileDetailsAsync(
                userId,
                cancellationToken);

        if (user is null)
        {
            return null;
        }

        var profile = MapProfile(user);

        var profilePicture =
            await GetProfilePictureAsync(
                userId,
                cancellationToken);

        var activities =
            await GetRecentActivitiesAsync(
                userId,
                cancellationToken);

        return new ProfileDetailResponseDto
        {
            Profile = profile,

            ProfilePicture = profilePicture,

            Preferences = null,

            RecentActivities = activities
        };
    }


    // ============================================================
    // Get Profile Summary
    // ============================================================

    public async Task<ProfileSummaryResponseDto?>
        GetProfileSummaryAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
    {
        var user =
            await _unitOfWork.Users.GetWithProfileDetailsAsync(
                userId,
                cancellationToken);

        if (user is null)
        {
            return null;
        }

        var profilePicture =
            await GetProfilePictureAsync(
                userId,
                cancellationToken);

        return new ProfileSummaryResponseDto
        {
            Id = user.Id,

            EmployeeId = user.EmployeeId,

            FullName = user.FullName,

            Email = user.Email,

            DepartmentName =
                user.Department?.Name,

            PrimaryRole =
                user.UserRoles
                    .Select(x => x.Role?.Name)
                    .FirstOrDefault(
                        x => !string.IsNullOrWhiteSpace(x)),

            ProfilePictureUrl =
                profilePicture?.FileUrl
        };
    }


    // ============================================================
    // Update Profile
    // ============================================================

    public async Task<ProfileResponseDto?>
        UpdateProfileAsync(
            Guid userId,
            UpdateProfileRequestDto request,
            CancellationToken cancellationToken = default)
    {
        var user =
            await _unitOfWork.Users.GetByIdAsync(
                userId,
                cancellationToken);

        if (user is null ||
            user.IsDeleted)
        {
            return null;
        }

        user.UpdateProfile(
            request.FullName,
            request.Email,
            request.PhoneNumber);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        var updatedUser =
            await _unitOfWork.Users.GetWithProfileDetailsAsync(
                userId,
                cancellationToken);

        return updatedUser is null
            ? null
            : MapProfile(updatedUser);
    }


    // ============================================================
    // Update Profile Picture
    // ============================================================

    public async Task<ProfilePictureResponseDto?>
        UpdateProfilePictureAsync(
            Guid userId,
            UpdateProfilePictureRequestDto request,
            CancellationToken cancellationToken = default)
    {
        var user =
            await _unitOfWork.Users.GetByIdAsync(
                userId,
                cancellationToken);

        if (user is null ||
            user.IsDeleted)
        {
            return null;
        }

        var existingFiles =
            await _unitOfWork.FileAttachments
                .GetActiveByEntityAsync(
                    nameof(User),
                    userId,
                    cancellationToken);

        var existingPicture =
            existingFiles
                .FirstOrDefault(
                    x =>
                        x.FileType ==
                        FileAttachmentType.ProfilePicture);

        if (existingPicture is not null)
        {
            existingPicture.Archive();
        }

        var extension =
            Path.GetExtension(request.FileName);

        var storedFileName =
            $"{Guid.NewGuid():N}{extension}";

        var fileAttachment =
            FileAttachment.Create(
                uploadedById: userId,
                entityName: nameof(User),
                entityId: userId,
                fileName: request.FileName,
                storedFileName: storedFileName,
                filePath: request.FilePath,
                contentType: request.ContentType,
                fileExtension: extension,
                fileSize: request.FileSize,
                fileType: FileAttachmentType.ProfilePicture,
                description: "User profile picture");

        await _unitOfWork.FileAttachments.AddAsync(
            fileAttachment,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return new ProfilePictureResponseDto
        {
            FileId = fileAttachment.Id,

            FileName = fileAttachment.FileName,

            ContentType = fileAttachment.ContentType,

            FileSize = fileAttachment.FileSize,

            FileUrl = fileAttachment.FilePath,

            UploadedAt = fileAttachment.UploadedAt
        };
    }


    // ============================================================
    // Update Preferences
    // ============================================================

    public Task<ProfilePreferencesResponseDto?>
        UpdatePreferencesAsync(
            Guid userId,
            UpdateProfilePreferencesRequestDto request,
            CancellationToken cancellationToken = default)
    {
        /*
         * The current User entity does not contain:
         *
         * Language
         * TimeZone
         * EmailNotificationsEnabled
         * SystemNotificationsEnabled
         *
         * Therefore these values cannot currently be persisted
         * correctly.
         *
         * Do not silently store them in memory.
         *
         * Add a UserPreference/ProfilePreference entity before
         * enabling this operation.
         */

        throw new NotSupportedException(
            "Profile preferences require a persistent user preference entity.");
    }


    // ============================================================
    // Map User -> Profile Response
    // ============================================================

    private static ProfileResponseDto MapProfile(User user)
    {
        return new ProfileResponseDto
        {
            Id = user.Id,

            EmployeeId = user.EmployeeId,

            FullName = user.FullName,

            Email = user.Email,

            PhoneNumber = user.PhoneNumber,

            Username = user.Username,

            DepartmentId = user.DepartmentId,

            DepartmentCode =
                user.Department?.Code ?? string.Empty,

            DepartmentName =
                user.Department?.Name ?? string.Empty,

            Roles = user.UserRoles
                .Where(x => x.Role is not null)
                .Select(x => new ProfileRoleDto
                {
                    Id = x.Role.Id,

                    Name = x.Role.Name,

                    Code = x.Role.Code,

                    Description = x.Role.Description,

                    IsSystemRole = x.Role.IsSystemRole
                })
                .ToList(),

            IsActive = user.IsActive,

            LastLoginAt = user.LastLoginAt
        };
    }


    // ============================================================
    // Get Profile Picture
    // ============================================================

    private async Task<ProfilePictureResponseDto?>
        GetProfilePictureAsync(
            Guid userId,
            CancellationToken cancellationToken)
    {
        var files =
            await _unitOfWork.FileAttachments
                .GetActiveByEntityAsync(
                    nameof(User),
                    userId,
                    cancellationToken);

        var picture =
            files.FirstOrDefault(
                x =>
                    x.FileType ==
                    FileAttachmentType.ProfilePicture);

        if (picture is null)
        {
            return null;
        }

        return new ProfilePictureResponseDto
        {
            FileId = picture.Id,

            FileName = picture.FileName,

            ContentType = picture.ContentType,

            FileSize = picture.FileSize,

            FileUrl = picture.FilePath,

            UploadedAt = picture.UploadedAt
        };
    }


    // ============================================================
    // Get Recent Activities
    // ============================================================

    private async Task<List<ProfileActivityDto>>
        GetRecentActivitiesAsync(
            Guid userId,
            CancellationToken cancellationToken)
    {
        var logs =
            await _unitOfWork.AuditLogs
                .GetByUserIdAsync(
                    userId,
                    cancellationToken);

        return logs
            .OrderByDescending(x => x.Timestamp)
            .Take(10)
            .Select(
                x => new ProfileActivityDto
                {
                    Id = x.Id,

                    Action =
                        x.Action.ToString(),

                    EntityName =
                        x.EntityName,

                    Description =
                        x.Description,

                    IpAddress =
                        x.IpAddress,

                    Timestamp =
                        x.Timestamp,

                    IsSuccessful =
                        x.IsSuccessful
                })
            .ToList();
    }
}