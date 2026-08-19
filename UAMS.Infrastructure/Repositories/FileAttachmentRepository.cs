using Microsoft.EntityFrameworkCore;
using UAMS.Application.Interfaces.Repositories;
using UAMS.Domain.Entities.FileAttachments;
using UAMS.Domain.Enums;
using UAMS.Infrastructure.Persistence;

namespace UAMS.Infrastructure.Repositories;

public class FileAttachmentRepository
    : GenericRepository<FileAttachment>, IFileAttachmentRepository
{
    public FileAttachmentRepository(UAMSDbContext context)
        : base(context)
    {
    }


    // ================================================================
    // Get File By Original File Name
    // ================================================================

    public virtual async Task<FileAttachment?>
        GetByFileNameAsync(
            string fileName,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                file =>
                    file.FileName == fileName,
                cancellationToken);
    }


    // ================================================================
    // Get File By Stored File Name
    // ================================================================

    public virtual async Task<FileAttachment?>
        GetByStoredFileNameAsync(
            string storedFileName,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                file =>
                    file.StoredFileName == storedFileName,
                cancellationToken);
    }


    // ================================================================
    // Get Files By Entity
    // ================================================================

    public virtual async Task<IReadOnlyList<FileAttachment>>
        GetByEntityAsync(
            string entityName,
            Guid entityId,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(file =>
                file.EntityName == entityName &&
                file.EntityId == entityId)
            .OrderByDescending(file =>
                file.UploadedAt)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Files Uploaded By User
    // ================================================================

    public virtual async Task<IReadOnlyList<FileAttachment>>
        GetByUploadedByIdAsync(
            Guid uploadedById,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(file =>
                file.UploadedById == uploadedById)
            .OrderByDescending(file =>
                file.UploadedAt)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Files By File Type
    // ================================================================

    public virtual async Task<IReadOnlyList<FileAttachment>>
        GetByFileTypeAsync(
            FileAttachmentType fileType,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(file =>
                file.FileType == fileType)
            .OrderByDescending(file =>
                file.UploadedAt)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Files By Status
    // ================================================================

    public virtual async Task<IReadOnlyList<FileAttachment>>
        GetByStatusAsync(
            FileAttachmentStatus status,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(file =>
                file.Status == status)
            .OrderByDescending(file =>
                file.UploadedAt)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Active Files By Entity
    // ================================================================

    public virtual async Task<IReadOnlyList<FileAttachment>>
        GetActiveByEntityAsync(
            string entityName,
            Guid entityId,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(file =>
                file.EntityName == entityName &&
                file.EntityId == entityId &&
                file.Status == FileAttachmentStatus.Active &&
                file.IsActive &&
                !file.IsDeleted)
            .OrderByDescending(file =>
                file.UploadedAt)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Active Files Uploaded By User
    // ================================================================

    public virtual async Task<IReadOnlyList<FileAttachment>>
        GetActiveByUploadedByIdAsync(
            Guid uploadedById,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(file =>
                file.UploadedById == uploadedById &&
                file.Status == FileAttachmentStatus.Active &&
                file.IsActive &&
                !file.IsDeleted)
            .OrderByDescending(file =>
                file.UploadedAt)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get File By Checksum
    // ================================================================

    public virtual async Task<FileAttachment?>
        GetByChecksumAsync(
            string checksum,
            CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(checksum))
        {
            return null;
        }

        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                file =>
                    file.Checksum == checksum &&
                    !file.IsDeleted,
                cancellationToken);
    }


    // ================================================================
    // Check Whether File Exists By Checksum
    // ================================================================

    public virtual async Task<bool>
        ExistsByChecksumAsync(
            string checksum,
            CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(checksum))
        {
            return false;
        }

        return await DbSet
            .AnyAsync(
                file =>
                    file.Checksum == checksum &&
                    !file.IsDeleted,
                cancellationToken);
    }


    // ================================================================
    // Get Total File Size Uploaded By User
    // ================================================================

    public virtual async Task<long>
        GetTotalFileSizeByUploadedByIdAsync(
            Guid uploadedById,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(file =>
                file.UploadedById == uploadedById &&
                file.Status == FileAttachmentStatus.Active &&
                file.IsActive &&
                !file.IsDeleted)
            .SumAsync(
                file => file.FileSize,
                cancellationToken);
    }
}