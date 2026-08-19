using UAMS.Domain.Entities.FileAttachments;
using UAMS.Domain.Enums;

namespace UAMS.Application.Interfaces.Repositories;

public interface IFileAttachmentRepository
    : IRepository<FileAttachment>
{
    // ================================================================
    // Get File By Name
    // ================================================================

    Task<FileAttachment?> GetByFileNameAsync(
        string fileName,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get File By Stored File Name
    // ================================================================

    Task<FileAttachment?> GetByStoredFileNameAsync(
        string storedFileName,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Files By Entity
    // ================================================================

    Task<IReadOnlyList<FileAttachment>> GetByEntityAsync(
        string entityName,
        Guid entityId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Files Uploaded By User
    // ================================================================

    Task<IReadOnlyList<FileAttachment>> GetByUploadedByIdAsync(
        Guid uploadedById,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Files By Type
    // ================================================================

    Task<IReadOnlyList<FileAttachment>> GetByFileTypeAsync(
        FileAttachmentType fileType,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Files By Status
    // ================================================================

    Task<IReadOnlyList<FileAttachment>> GetByStatusAsync(
        FileAttachmentStatus status,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Active Files By Entity
    // ================================================================

    Task<IReadOnlyList<FileAttachment>> GetActiveByEntityAsync(
        string entityName,
        Guid entityId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Active Files Uploaded By User
    // ================================================================

    Task<IReadOnlyList<FileAttachment>> GetActiveByUploadedByIdAsync(
        Guid uploadedById,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get File By Checksum
    // ================================================================

    Task<FileAttachment?> GetByChecksumAsync(
        string checksum,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Check Whether File Exists By Checksum
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