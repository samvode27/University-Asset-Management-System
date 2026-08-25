using UAMS.Application.DTOs.FileAttachments.Requests;
using UAMS.Application.DTOs.FileAttachments.Responses;

namespace UAMS.Application.Interfaces.Services;

public interface IFileAttachmentService
{
    // ================================================================
    // Get File Attachment By ID
    // ================================================================

    Task<FileAttachmentResponseDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get File Attachment Details
    // ================================================================

    Task<FileAttachmentDetailResponseDto?> GetDetailsAsync(
        Guid id,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get File Attachment By File Name
    // ================================================================

    Task<FileAttachmentResponseDto?> GetByFileNameAsync(
        string fileName,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get File Attachment By Stored File Name
    // ================================================================

    Task<FileAttachmentResponseDto?> GetByStoredFileNameAsync(
        string storedFileName,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Files By Entity
    // ================================================================

    Task<IReadOnlyCollection<FileAttachmentResponseDto>> GetByEntityAsync(
        string entityName,
        Guid entityId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Active Files By Entity
    // ================================================================

    Task<IReadOnlyCollection<FileAttachmentResponseDto>> GetActiveByEntityAsync(
        string entityName,
        Guid entityId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Files Uploaded By User
    // ================================================================

    Task<IReadOnlyCollection<FileAttachmentResponseDto>> GetByUploadedByIdAsync(
        Guid uploadedById,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Active Files Uploaded By User
    // ================================================================

    Task<IReadOnlyCollection<FileAttachmentResponseDto>>
        GetActiveByUploadedByIdAsync(
            Guid uploadedById,
            CancellationToken cancellationToken = default);


    // ================================================================
    // Get Files By Type
    // ================================================================

    Task<IReadOnlyCollection<FileAttachmentResponseDto>> GetByFileTypeAsync(
        Domain.Enums.FileAttachmentType fileType,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Files By Status
    // ================================================================

    Task<IReadOnlyCollection<FileAttachmentResponseDto>> GetByStatusAsync(
        Domain.Enums.FileAttachmentStatus status,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Files By Checksum
    // ================================================================

    Task<FileAttachmentResponseDto?> GetByChecksumAsync(
        string checksum,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get All With Filtering / Pagination
    // ================================================================

    Task<FileAttachmentListResponseDto> GetAllAsync(
        FileAttachmentFilterRequestDto request,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Upload File
    // ================================================================

    Task<FileAttachmentResponseDto> UploadAsync(
        UploadFileAttachmentRequestDto request,
        Guid uploadedById,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Update File Metadata
    // ================================================================

    Task<FileAttachmentResponseDto?> UpdateAsync(
        Guid id,
        UpdateFileAttachmentRequestDto request,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Archive File
    // ================================================================

    Task<FileAttachmentResponseDto?> ArchiveAsync(
        Guid id,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Restore File
    // ================================================================

    Task<FileAttachmentResponseDto?> RestoreAsync(
        Guid id,
        Guid restoredById,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Delete File
    // ================================================================

    Task<FileAttachmentResponseDto?> DeleteAsync(
        Guid id,
        Guid deletedById,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Activate File
    // ================================================================

    Task<FileAttachmentResponseDto?> ActivateAsync(
        Guid id,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Deactivate File
    // ================================================================

    Task<FileAttachmentResponseDto?> DeactivateAsync(
        Guid id,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Checksum Exists
    // ================================================================

    Task<bool> ExistsByChecksumAsync(
        string checksum,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Total File Size Uploaded By User
    // ================================================================

    Task<long> GetTotalFileSizeByUploadedByIdAsync(
        Guid uploadedById,
        CancellationToken cancellationToken = default);
}